using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    [SerializeField] private float moveSpeed = 5;
    private float sensitivity => PlayerPrefs.GetFloat("Sensitivity", 2.5f);
    [SerializeField] private float _jumpHeight = 5;
    private float jumpHeight => _jumpHeight;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float airMoveAmount = 40;
    [SerializeField] private float gravity = 15;

    [SerializeField] private Transform camPos;
    [SerializeField] private AudioSource source;
    private Rigidbody rb;

    private Transform cam;
    private float x, z;
    public float camX, camY;
    private float camRot;

    Vector3 lastDir;
    Vector3 moveDir;
    Vector3 controllerMoveDir;
    Vector3 yVel;
    Vector3 groundNormal;

    private bool grounded => Grounded();
    private bool groundedPrevFrame;
    public bool isMovingAndGrounded => grounded && moving;

    private bool moving;
    private bool jumping;
    private bool running;

    public bool swinging;
    public bool traveling;
    private bool dashing;

    private bool ceilChecked;

    public enum MoveState { GroundMove, AirMove, Grappling, Dashing }
    public MoveState moveState;

    void Awake()
    {
        cam = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (moveSpeed > maxSpeed)
            maxSpeed = moveSpeed;
    }

    void Update()
    {
        if (Player.Instance.paused || Player.Instance.dead)
            return;

        SetMoveState();
        PlayerInput();
        Movement();
        CheckLanded();
    }
    private void LateUpdate()
    {
        CamMovement();
    }
    public void ResetForces()
    {
        lastDir = Vector3.zero;
        controllerMoveDir = Vector3.zero;
        yVel = Vector3.zero;
    }
    void SetMoveState()
    {
        moveState = traveling ? MoveState.Grappling : (grounded ? (swinging ? MoveState.Grappling : (dashing ? MoveState.Dashing : MoveState.GroundMove)) : (swinging ? MoveState.Grappling : (dashing ? MoveState.Dashing : MoveState.AirMove)));
    }
    void PlayerInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        camX += Input.GetAxis("Mouse X") * sensitivity * Time.timeScale;
        camY += Input.GetAxis("Mouse Y") * sensitivity * Time.timeScale;
        camY = Mathf.Clamp(camY, -90, 90);

        running = Input.GetKey(KeyCode.LeftShift);

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            jumping = true;
            yVel.y = jumpHeight;

            lastDir += controller.velocity;

            Invoke(nameof(ResetJump), 0.4f);
        }
    }
    void CheckLanded()
    {
        if (grounded && !groundedPrevFrame)
        {
            AudioManager.Instance.PlayAudioOnGlobalSource(AudioManager.Instance.playerLand);
        }
        groundedPrevFrame = grounded;
    }
    void Movement()
    {
        SlopeCheck();
        //moveDir = x * transform.right + z * transform.forward;

        moveDir = (Vector3.Cross(transform.right, groundNormal) * z - Vector3.Cross(transform.forward, groundNormal) * x).normalized;

        moveDir *= moveSpeed;

        Debug.DrawRay(transform.position - Vector3.up, moveDir, Color.red, 0.1f);

        moving = new Vector3(moveDir.x, 0, moveDir.z).magnitude > 0;

        if(!source.isPlaying && isMovingAndGrounded)
        {
            source.Play();
        }
        else if(source.isPlaying && !isMovingAndGrounded)
        {
            source.Stop();
        }

        switch (moveState)
        {
            case MoveState.GroundMove:
                MoveGround();
                break;
            case MoveState.AirMove:
                MoveAir();
                break;
            case MoveState.Grappling:
                CaptureGrapple();
                if (traveling)
                    controller.Move(controllerMoveDir * Time.deltaTime);
                return;
            case MoveState.Dashing:
                controller.Move(controllerMoveDir * Time.deltaTime);
                lastDir = new Vector3(controller.velocity.x, 0, controller.velocity.z);
                yVel.y = 0;

                lastDir.x = Mathf.Abs(controller.velocity.x) < Mathf.Abs(lastDir.x) ? controller.velocity.x : lastDir.x;
                lastDir.z = Mathf.Abs(controller.velocity.z) < Mathf.Abs(lastDir.z) ? controller.velocity.z : lastDir.z;
                return;
            default:
                break;
        }

        if (!controller.enabled)
            return;
        controllerMoveDir += yVel;
        controller.Move(controllerMoveDir * Time.deltaTime);
    }
    void MoveGround()
    {
        if (!jumping)
            yVel.y = 0;

        if (!moving && !jumping)
        {
            lastDir = Vector3.Lerp(lastDir, Vector3.zero, Time.deltaTime * 10);
            controller.Move(lastDir * Time.deltaTime);
        }
        if (moving)
            lastDir = new Vector3(moveDir.x, 0, moveDir.z);

        controllerMoveDir = moveDir;
    }
    void MoveAir()
    {
        if (!controller.enabled)
            return;

        moveDir.y = 0;
        if (moving)
        {
            lastDir += moveDir * Time.deltaTime * airMoveAmount;

            if (lastDir.magnitude > maxSpeed)
                lastDir *= maxSpeed / lastDir.magnitude;
        }
        else if (!jumping)
        {
            lastDir.x = Mathf.Abs(controller.velocity.x) < Mathf.Abs(lastDir.x) ? controller.velocity.x : lastDir.x;
            lastDir.z = Mathf.Abs(controller.velocity.z) < Mathf.Abs(lastDir.z) ? controller.velocity.z : lastDir.z;
        }
        CheckCeiling();

        yVel.y -= gravity * Time.deltaTime;
        controllerMoveDir = lastDir;
    }
    void CamMovement()
    {
        transform.rotation = Quaternion.Euler(0, camX, 0);

        camRot = Mathf.Lerp(camRot, x * -2.5f, Time.deltaTime * 5);

        camPos.rotation = Quaternion.Euler(-camY, camX, 0);

        camPos.localRotation *= Quaternion.Euler(0, 0, camRot);

        cam.SetPositionAndRotation(camPos.position, camPos.rotation);
    }
    void ResetJump()
    {
        jumping = false;
    }
    public void AddForce(Vector3 force)
    {
        jumping = true;

        controllerMoveDir += new Vector3(force.x, 0, force.z);
        yVel.y += force.y;

        Invoke(nameof(ResetJump), 0.2f);
    }
    public bool Grounded()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position, 0.35f, Vector3.down, out hit, 0.85f, ~LayerMask.GetMask("Player", "Ignore Player", "Ignore Raycast"));
    }
    void CheckCeiling()
    {
        if (controller.collisionFlags == CollisionFlags.Above)
        {
            if (!ceilChecked)
                yVel.y = -0.1f;
            ceilChecked = true;
        }
        else
            ceilChecked = false;
        //if (Physics.SphereCast(transform.position, 0.2f, Vector3.up, out RaycastHit hit, 1.2f, ~LayerMask.GetMask("Ignore Raycast", "Ignore Player", "Player")))
        //{
        //    if (!ceilChecked)
        //        yVel.y = -0.1f;
        //    ceilChecked = true;
        //}
        //else
        //    ceilChecked = false;
    }
    public void SlopeCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f, ~LayerMask.GetMask("Player", "Ignore Player", "Ignore Raycast")))
        {
            groundNormal = hit.normal;
        }
        else
            groundNormal = Vector3.up;
    }
    void CaptureGrapple()
    {
        Vector3 captureVelocity = swinging ? rb.velocity : controller.velocity;

        lastDir = new Vector3(captureVelocity.x, 0, captureVelocity.z);
        yVel.y = captureVelocity.y;
    }

    public void StartTravel(TravelPoint point)
    {
        traveling = true;
        controllerMoveDir = (point.transform.position - transform.position).normalized * point.force + Vector3.up;
    }
    public void EndTravel()
    {
        traveling = false;
    }
    public void StartSwing()
    {
        swinging = true;

        Vector3 cVel = controller.velocity;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        controller.enabled = false;
        rb.velocity = cVel + Vector3.up * 2.5f;
    }
    public void EndSwing()
    {
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rb.isKinematic = true;
        controller.enabled = true;

        jumping = true;
        swinging = false;
        Invoke(nameof(ResetJump), 0.2f);
    }
    public void ChangeDashSpeed(float speed = 10)
    {
        if (dashing)
        {
            controllerMoveDir = transform.forward * speed;
        }
    }
    public bool StartDash()
    {
        if (traveling || swinging)
            return false;

        dashing = true;
        controllerMoveDir = transform.forward * 30;

        return true;
    }
    public void EndDash()
    {
        dashing = false;
    }
}