using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEnemy
{
    public EnemyHandler handler;
    [SerializeField] protected bool active = true;
    [SerializeField] private bool register = true;

    public delegate void TakeDamage();
    public delegate void Death();

    public event TakeDamage OnTakeDamage;
    public event Death OnDeath;

    protected Vector3 playerPosition => Player.Instance.transform.position;
    protected Vector3 playerVector => playerPosition - (transform.position + originOffset);
    [SerializeField] private Vector3 originOffset = Vector3.up;

    protected NavMeshAgent agent;

    protected float attackCooldown = 1;

    protected float timer;

    [SerializeField] protected float damage = 10;

    public float health = 25;
    public float maxHealth;

    [SerializeField] private GameObject deathParticles;
    [SerializeField] private Vector3 particleOffset;
    [SerializeField] protected AudioSource source;
    [SerializeField] private AudioClip deathClip;

    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        maxHealth = health;
    }
    void Start()
    {
        //if (register)
        //    handler.enemies.Add(this);
    }

    public virtual void Chase()
    {
        if (!active)
            return;
        agent.SetDestination(playerPosition);
    }

    public virtual void Attack()
    {
        if (!active)
            return;
    }

    public bool Damage(float amount, Vector3 attackDirection)
    {
        OnTakeDamage?.Invoke();
        if (health <= 0)
            return true;
        health -= amount;

        if(health <= 0)
        {
            //AudioClip[] clips = new AudioClip[] { AudioManager.Instance.bloodExplosion, deathClip };

            ObjectPool.Instance.GetPooledObject(deathParticles, transform.position + particleOffset, Quaternion.LookRotation(attackDirection))/*.GetComponent<AudioEffectPlayer>().Init(clips)*/;

            Die();

            return true;
        }
        return false;
    }
    public virtual void Die()
    {
        OnDeath?.Invoke();
        if(register && handler != null)
            handler.RemoveEnemy(this);
    }
}