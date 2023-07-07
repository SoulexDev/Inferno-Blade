using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private int openRequirement = 1;
    private int _openAmount = 0;
    private int openAmount { get { return _openAmount; } set { _openAmount = value; if (_openAmount >= openRequirement) SetOpen(); else SetClosed(); } }

    private Vector3 startPos;
    private Vector3 endPos;
    private bool _open;
    [SerializeField] private AudioSource source;
    [HideInInspector] public bool open { get { return _open; } set { _open = value; if (_open) SetOpen(); else SetClosed(); } }

    private void Awake()
    {
        startPos = transform.position;
        endPos = startPos + transform.up * 4;
    }
    public void AddOpenAmount()
    {
        openAmount++;
    }
    public void SetOpen()
    {
        StopAllCoroutines();
        StartCoroutine(Open());
    }
    public void SetClosed()
    {
        StopAllCoroutines();
        StartCoroutine(Close());
    }

    IEnumerator Open()
    {
        source.Play();
        while (transform.position != endPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator Close()
    {
        while (transform.position != startPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            yield return null;
        }
    }
}