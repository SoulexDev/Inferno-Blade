using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : GroundEnemy
{
    [SerializeField] private Collider col;

    private float moveState;
    private float currentMoveState => anims.GetFloat("MoveState");

    public override void Update()
    {
        base.Update();
        switch (state)
        {
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                if (!attacking)
                {
                    attacking = true;
                    Attack();
                }
                break;
            default:
                break;
        }
        UpdateAnims();
    }
    void UpdateAnims()
    {
        moveState = agent.velocity.magnitude == 0 ? 0 : 1;
        anims.SetFloat("MoveState", Mathf.MoveTowards(currentMoveState, moveState, Time.deltaTime * 25));
    }
    public override void Attack()
    {
        StartCoroutine(DashAttack());
    }
    public override void Chase()
    {
        base.Chase();
    }
    IEnumerator DashAttack()
    {
        bool attacked = false;
        agent.destination = playerPosition - playerVector.normalized;

        transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(playerVector).eulerAngles.y, 0);

        Vector3 startPos = transform.position;
        Vector3 endPos = agent.destination;

        agent.velocity = playerVector.normalized * 25;
        float attackTimer = 0;

        while (attackTimer < 0.1f)
        {
            attackTimer += Time.deltaTime;
            if (!attacked)
            {
                RaycastHit[] hits = Physics.SphereCastAll(startPos, 0.5f, endPos - startPos, 5);
                foreach (var hit in hits)
                {
                    if (hit.transform.TryGetComponent(out IPlayer player))
                    {
                        player.Damage(damage);
                        attacked = true;
                    }
                }
            }
            yield return null;
        }

        agent.velocity = Vector3.zero;

        yield return new WaitForSeconds(3);
        attacking = false;
    }

    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}