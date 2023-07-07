using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisabler : MonoBehaviour
{
    private ParticleSystem particles;
    [SerializeField] private float disableTime = 0;
    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        var main = particles.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
    private void OnEnable()
    {
        particles.Play();
    }
    private void OnParticleSystemStopped()
    {
        Invoke(nameof(Disable), disableTime);
    }
    void Disable()
    {
        gameObject.SetActive(false);
    }
}