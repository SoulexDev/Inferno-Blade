using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.spawnPos = transform.position + Vector3.up;
            Player.Instance.xRot = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);
        }
    }
}