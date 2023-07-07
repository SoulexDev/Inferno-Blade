using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnEnable : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    private void OnEnable()
    {
        source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(source.clip);
    }
}