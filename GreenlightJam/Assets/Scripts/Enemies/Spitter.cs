using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitter : GroundEnemy
{
    [SerializeField] private float timeBetweenAttacks = 1.25f;
    [SerializeField] private Bullet spit;
    [SerializeField] private Transform spitSpawnPos;
    private float moveState;
    private float currentMoveState => anims.GetFloat("MoveState");

    private enum EnemyState { Back, Attack, Forward }
    private EnemyState enemyState;

    public override void Update()
    {
        if (!active)
            return;

        enemyState = playerVector.magnitude < 5.5f ? EnemyState.Back : (playerVector.magnitude > 12 ? EnemyState.Forward : EnemyState.Attack);

        switch (enemyState)
        {
            case EnemyState.Back:
                if (attacking)
                {
                    agent.isStopped = false;
                    attacking = false;
                    StopAllCoroutines();
                }

                BackAway();

                break;
            case EnemyState.Attack:
                if (!attacking)
                {
                    agent.isStopped = true;
                    StartCoroutine(TimeAttack());
                    attacking = true;
                }

                break;
            case EnemyState.Forward:
                if (attacking)
                {
                    agent.isStopped = false;
                    attacking = false;
                    StopAllCoroutines();
                }

                MoveForward();

                break;
            default:
                break;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, Quaternion.LookRotation(playerVector).eulerAngles.y, 0), Time.deltaTime * 5);

        UpdateAnims();
    }
    void BackAway()
    {
        agent.speed = 10;
        agent.destination = transform.position - playerVector.normalized * 5;
    }
    void MoveForward()
    {
        agent.speed = 5;
        agent.destination = playerPosition;
    }
    void UpdateAnims()
    {
        moveState = agent.velocity.magnitude == 0 ? 0.5f : (playerVector.magnitude < 4 ? 0 : 1);
        anims.SetFloat("MoveState", Mathf.MoveTowards(currentMoveState, moveState, Time.deltaTime * 10));
    }
    IEnumerator TimeAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
            Instantiate(spit, spitSpawnPos.position, Quaternion.LookRotation(playerVector)).Init(damage, Bullet.ShooterType.Enemy, 35);
        }
    }
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}