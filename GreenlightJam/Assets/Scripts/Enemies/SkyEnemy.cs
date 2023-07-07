using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyEnemy : Enemy
{
    [SerializeField] protected float height = 5;

    public override void Awake()
    {
        base.Awake();
        agent.baseOffset = height;
    }
    public virtual void Update()
    {

    }
}