using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //[SerializeField] private GameObject hitParticles;
    [SerializeField] private float speed = 40;
    //[SerializeField] private TrailRenderer trail;
    private Vector3 prevPos;
    private float damage;
    public enum ShooterType { Enemy, Player }
    private ShooterType shooterType;

    private float timer;

    private bool damaged;

    void Update()
    {
        if (gameObject.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer > 10)
            {
                timer = 0;
                gameObject.SetActive(false);
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
            }
        }

        transform.position += transform.forward * speed * Time.deltaTime;
        string ignoreLayer = shooterType == ShooterType.Enemy ? "Enemy" : "Player";

        if (Physics.Raycast(prevPos, transform.forward, out RaycastHit hit, (transform.position - prevPos).magnitude, ~LayerMask.GetMask("Ignore Raycast", ignoreLayer)))
        {
            if (shooterType == ShooterType.Enemy)
            {
                if (hit.transform.TryGetComponent(out IPlayer player))
                {
                    if(!damaged)
                        player.Damage(damage);

                    damaged = true;
                }
            }
            else
            {
                if (hit.transform.TryGetComponent(out IEnemy enemy))
                {
                    if (!damaged)
                        enemy.Damage(damage, transform.forward);

                    damaged = true;
                }
            }
        }
        prevPos = transform.position;
    }
    public void Init(float damage, ShooterType shooterType, float speed = 40)
    {
        damaged = false;
        transform.position += transform.forward * speed * Time.deltaTime;
        prevPos = transform.position;

        this.shooterType = shooterType;
        this.damage = damage;
        this.speed = speed;
    }
}