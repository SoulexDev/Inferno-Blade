using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoarder : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float screenSize = 0.1f;
    [SerializeField] private Vector3 RotOffset = Vector3.up * 180;
    [SerializeField] private bool keepSize = true;
    [SerializeField] private float FOV = 60;
    private void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (cam != null)
        {
            transform.rotation = Quaternion.LookRotation((cam.transform.position - transform.position).normalized, Vector3.up) * Quaternion.Euler(RotOffset);
            if (keepSize)
            {
                float distance = (cam.transform.position - transform.position).magnitude;
                float scale = distance * screenSize * FOV;
                transform.localScale = Vector3.one * scale;
            }
        }
    }
}