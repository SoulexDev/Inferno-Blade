using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    public Vector3 GetPos()
    {
        return transform.position + offset;
    }
}