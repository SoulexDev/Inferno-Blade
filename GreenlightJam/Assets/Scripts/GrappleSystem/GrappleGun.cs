using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    [SerializeField] private float grappleDist = 20;
    [SerializeField] private Transform gunTip;
    [SerializeField] private int ropeQuality;
    [SerializeField] private float waveHeight = 0.5f;
    [SerializeField] private float waveCount = 1.5f;
    [SerializeField] private AnimationCurve waveCurve;

    [SerializeField] private float springVelocity = 10;
    [SerializeField] private float springStrength = 800;
    [SerializeField] private float springDamper = 14;
    [SerializeField] private AudioSource source;

    private LineRenderer lineRend;
    private Player player => Player.Instance;
    private SpringJoint joint;

    private GrapplePoint point;

    private Vector3 grapplePointPos;
    private Vector3 startVector;

    private GrappleSpring spring;

    private bool traveling;
    private bool grappling;
    private bool travelingToEnemy;

    private bool swingPoint;

    private Quaternion lookRotation => point != null ? Quaternion.LookRotation(point.GetPos() - transform.position) : Quaternion.identity;    

    private void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        spring = new GrappleSpring();
        spring.SetTarget(0);
    }
    private void Update()
    {
        if (Player.Instance.dead || Player.Instance.paused)
            return;

        PlayerInput();
        AlignRotation();

        if (point == null && grappling)
        {
            EndGrapple();
            return;
        }

        if (traveling && (Vector3.Distance(point.GetPos(), transform.position) < 2 || Vector3.Dot(startVector, point.GetPos() - player.transform.position) <= 0))
        {
            EndGrapple();
        }
    }
    private void LateUpdate()
    {
        if (point == null)
            return;
        SetLinePositons();
    }
    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (point == null)
                StartGrapple();
            else
                EndGrapple();
        }
    }
    void SetLinePositons()
    {
        if (grappling)
        {
            spring.Update(Time.deltaTime);

            Vector3 up = lookRotation * Vector3.up;
            for (int i = 0; i < ropeQuality + 1; i++)
            {
                float delta = i / (float)ropeQuality;
                Vector3 offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * waveCurve.Evaluate(delta);

                lineRend.SetPosition(i, Vector3.Lerp(gunTip.position, point.GetPos(), delta) + offset);
            }
        }
    }
    void AlignRotation()
    {
        Quaternion lookRot = grappling ? lookRotation : transform.parent.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5);
    }
    void StartGrapple()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, grappleDist))
        {
            if (hit.transform.TryGetComponent(out point))
            {
                if(point is SwingPoint)
                {
                    AudioManager.Instance.PlayAudioOnGlobalSource(AudioManager.Instance.grappleRed);
                    swingPoint = true;

                    player.controller.StartSwing();

                    grappling = true;

                    if (joint == null)
                        joint = player.gameObject.AddComponent<SpringJoint>();

                    grapplePointPos = point.GetPos();

                    joint.autoConfigureConnectedAnchor = false;
                    joint.connectedAnchor = grapplePointPos;

                    float ropeLength = Vector3.Distance(player.transform.position, grapplePointPos);
                    joint.maxDistance = ropeLength * 0.5f;
                    joint.minDistance = ropeLength * 0.25f;

                    joint.spring = 4.5f;
                    joint.damper = 7;
                    joint.massScale = 4.5f;
                }
                else if(point is TravelPoint)
                {
                    AudioManager.Instance.PlayAudioOnGlobalSource(AudioManager.Instance.grappleBlue);
                    source.Play();

                    player.controller.StartTravel(point as TravelPoint);
                    grappling = true;
                    traveling = true;

                    if (hit.transform.TryGetComponent(out IEnemy enemy))
                        travelingToEnemy = true;

                    startVector = point.GetPos() - player.transform.position;
                }

                spring.SetVelocity(springVelocity);

                lineRend.positionCount = ropeQuality + 1;

                spring.SetDamper(springDamper);
                spring.SetStrength(springStrength);
            }
        }
    }
    void EndGrapple()
    {
        AudioManager.Instance.PlayAudioOnGlobalSource(AudioManager.Instance.grappleRelease);
        grappling = false;
        source.Stop();

        if (swingPoint)
        {
            player.controller.EndSwing();
            Destroy(joint);

            swingPoint = false;
        }
        else
        {
            traveling = false;
            player.controller.EndTravel();

            if (travelingToEnemy)
            {
                player.sword.AttemptAttack();
                //player.controller.ResetForces();
                player.controller.AddForce(Vector3.up * 8 + player.transform.forward * 3);
                travelingToEnemy = false;
            }
        }

        point = null;
        spring.Reset();
        lineRend.positionCount = 0;
    }
}