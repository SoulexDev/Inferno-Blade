using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private Animator anims;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadius = 1.5f;

    [SerializeField] private Image dashImg;
    [SerializeField] private Image chargeCooldownImg;
    [SerializeField] private Image chargeImg;

    [SerializeField] private float dashRefillTime = 3;
    [SerializeField] private float chargeRefillTime = 5;

    [SerializeField] private GameObject sword;
    [SerializeField] private TrailRenderer trail;
    private float dashTimer = 1;
    private float chargeTimer = 1;
    private float chargeAmount = 0;

    private Player player => Player.Instance;
    private bool dashing;
    private bool chargeDash;
    private bool attacking;
    private bool canCharge;
    private bool charging;

    private bool low = true;

    private int currentDashAmount = 3;

    private AudioManager audioManager => AudioManager.Instance;

    private void Update()
    {
        if (Player.Instance.dead || Player.Instance.paused)
            return;

        PlayerInput();
        if(chargeTimer < 1)
        {
            chargeTimer += Time.deltaTime / chargeRefillTime;
            chargeCooldownImg.fillAmount = chargeTimer;
        }
        else if (!canCharge)
        {
            chargeCooldownImg.fillAmount = 1;
            canCharge = true;
        }

        if (dashTimer < 1 && chargeDash)
        {
            dashTimer += Time.deltaTime / dashRefillTime;
            dashImg.fillAmount = dashTimer;
        }
        else if(chargeDash)
        {
            dashImg.fillAmount = 1;
            chargeDash = false;
            currentDashAmount = 3;
        }

        if (charging)
        {
            if (chargeAmount < 1)
            {
                chargeAmount += Time.deltaTime;
                chargeImg.fillAmount = chargeAmount;
            }
            else
                chargeImg.fillAmount = 1;
        }
    }
    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing && !chargeDash)
        {
            dashing = true;
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttemptAttack();
        }

        if (Input.GetKeyDown(KeyCode.Q) && !attacking && canCharge)
        {
            anims.SetBool("Charge", true);
            charging = true;
        }

        if (Input.GetKeyUp(KeyCode.Q) && !attacking && canCharge && charging)
        {
            anims.SetBool("Charge", false);

            if(chargeAmount > 1)
            {
                GameObject newSword = ObjectPool.Instance.GetPooledObject(sword, Camera.main.transform.position, Quaternion.LookRotation(Camera.main.transform.forward));

                newSword.GetComponent<SwordProjectile>().Init(50, damage * 2);

                chargeTimer = 0;

                canCharge = false;
            }

            charging = false;
            chargeAmount = 0;
            chargeImg.fillAmount = 0;
        }
    }
    IEnumerator Dash()
    {
        if (!player.controller.StartDash())
        {
            dashing = false;
            yield break;
        }

        currentDashAmount--;

        if (currentDashAmount <= 0)
            chargeDash = true;

        dashImg.fillAmount = (float)currentDashAmount/3;

        dashTimer = 0;

        bool attacked = false;
        float attackTimer = 0;
        Vector3 lastPos = player.transform.position;

        audioManager.PlayAudioOnGlobalSource(audioManager.playerDash);

        while (attackTimer < 0.15f)
        {
            attackTimer += Time.deltaTime;

            if (!attacked)
            {
                RaycastHit[] hits = Physics.SphereCastAll(lastPos, 0.5f, player.transform.position - lastPos, (player.transform.position - lastPos).magnitude);
                foreach (var hit in hits)
                {
                    if (hit.transform.TryGetComponent(out IEnemy enemy))
                    {
                        enemy.Damage(damage, player.transform.forward);
                        attacked = true;

                        player.controller.ChangeDashSpeed();
                    }
                }
            }
            
            yield return null;
        }

        player.controller.EndDash();
        dashing = false;
    }
    public void AttemptAttack()
    {
        if (!attacking && !charging)
        {
            attacking = true;
            Attack();
        }
    }
    void Attack()
    {
        audioManager.PlayAudioOnGlobalSource(audioManager.swordSwings[Random.Range(0, audioManager.swordSwings.Length)]);

        low = !low;
        anims.SetTrigger("Swing");

        trail.emitting = true;
    }
    public void AttackEvent()
    {
        trail.emitting = false;
        Collider[] cols = Physics.OverlapSphere(attackPos.position, attackRadius, ~LayerMask.GetMask("Ignore Raycast", "Ignore Player", "Player"));
        foreach (var col in cols)
        {
            if(col.TryGetComponent(out IEnemy enemy))
            {
                enemy.Damage(damage, player.transform.forward);
            }
        }
    }
    public void ResetAttack()
    {
        attacking = false;
    }
}