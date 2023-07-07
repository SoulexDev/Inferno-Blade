using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectile : MonoBehaviour
{
    private float speed = 10;
    private float damage = 10;
    private Vector3 prevPos;
    private float timer;
    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
        Vector3 direction = transform.position - prevPos;
        if (Physics.Raycast(prevPos, direction, out RaycastHit hit, direction.magnitude + 2, ~LayerMask.GetMask("Ignore Raycast", "Player")))
        {
            if(hit.transform.TryGetComponent(out IEnemy enemy))
            {
                enemy.Damage(damage, direction);
            }
            else
                gameObject.SetActive(false);
        }
        timer += Time.deltaTime;
        if (timer > 10)
            gameObject.SetActive(false);
        prevPos = transform.position;
    }

    public void Init(float speed, float damage)
    {
        timer = 0;
        prevPos = transform.position;
        this.speed = speed;
        this.damage = damage;
    }
}