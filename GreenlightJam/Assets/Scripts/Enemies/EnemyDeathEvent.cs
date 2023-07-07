using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDeathEvent : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    public UnityEvent deathEvent;

    private void Awake()
    {
        enemy.OnDeath += Enemy_OnDeath;
    }

    private void Enemy_OnDeath()
    {
        deathEvent?.Invoke();
    }
}