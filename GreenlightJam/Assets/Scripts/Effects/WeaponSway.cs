using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float swayAmount = 0.25f;
    [SerializeField] private float swaySpeed = 0.5f;
    private Vector3 startPos;

    bool sway => Player.Instance.controller.isMovingAndGrounded;
    private void Awake()
    {
        startPos = transform.localPosition;
    }
    private void Update()
    {
        if (sway)
            transform.localPosition = Vector3.Lerp(transform.localPosition, 
                startPos + (Vector3.right * Mathf.Sin(Time.time * swaySpeed) + Vector3.up * (Mathf.Cos(Time.time * 2 * swaySpeed) + 1) / 2) * swayAmount, 
                Time.deltaTime * 10);
        else
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * 5);
    }
}