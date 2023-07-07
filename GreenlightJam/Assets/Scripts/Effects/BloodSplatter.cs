using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    [SerializeField] private GameObject bloodDecal;
    private ParticleSystem particles;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int collisionAmount = particles.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < collisionAmount; i++)
        {
            if (!Physics.Raycast(collisionEvents[i].intersection + collisionEvents[i].normal * 0.5f, -collisionEvents[i].normal, out RaycastHit hit, 1, particles.collision.collidesWith))
                hit.point = transform.position;

            GameObject newDecal = ObjectPool.Instance.AddToBloodPool(bloodDecal, hit.point + hit.normal * 0.001f, Quaternion.identity);

            newDecal.transform.up = hit.normal;
            newDecal.transform.Rotate(Vector3.up, Random.Range(-360, 360), Space.Self);
        }
    }
}