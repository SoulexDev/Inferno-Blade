using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    [SerializeField] private bool rotate = false;
    [SerializeField] protected float attackDist = 3;
    protected bool attacking;
    protected enum State { Chase, Attack }
    protected State state;

    protected Animator anims;

    public override void Awake()
    {
        base.Awake();
        anims = GetComponent<Animator>();
    }
    public virtual void Update()
    {
        if (!active)
            return;
        if(rotate)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, Quaternion.LookRotation(playerVector).eulerAngles.y, 0), Time.deltaTime * 5);

        state = playerVector.magnitude > attackDist && !attacking ? State.Chase : State.Attack;
    }
}