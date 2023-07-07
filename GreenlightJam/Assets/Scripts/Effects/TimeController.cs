using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;
    [SerializeField] private float freezeTime = 0.25f;
    [SerializeField] private float slowTime = 2;
    [SerializeField] private AnimationCurve timeSlowCurve;
    [SerializeField] private AudioMixer sfxMixer;
    private void Awake()
    {
        Instance = this;
    }
    public void StartFreezeTime()
    {
        StartCoroutine(FreezeTime());
    }
    IEnumerator FreezeTime()
    {
        float startTime = Time.realtimeSinceStartup;
        Time.timeScale = 0;
        while (Time.realtimeSinceStartup < startTime + freezeTime)
        {
            if (Player.Instance.paused)
                freezeTime += Time.deltaTime;
            yield return null;
        }
        Time.timeScale = 1;
    }

    public void StartSlowTime()
    {
        StartCoroutine(SlowTime());
    }
    IEnumerator SlowTime()
    {
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < startTime + slowTime)
        {
            if (Player.Instance.paused)
                slowTime += Time.unscaledDeltaTime;
            Time.timeScale = timeSlowCurve.Evaluate((Time.realtimeSinceStartup - startTime) / slowTime);
            sfxMixer.SetFloat("Pitch", Time.timeScale);
            yield return null;
        }
        sfxMixer.SetFloat("Pitch", 1);
        Time.timeScale = 1;
    }
}