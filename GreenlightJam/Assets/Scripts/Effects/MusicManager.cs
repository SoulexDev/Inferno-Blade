using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private AudioSource[] sources;

    [SerializeField] private AudioSource baseLayer;
    [SerializeField] private AudioSource overlayLayer;

    [SerializeField] private float fadeLength = 1;
    private int currentSourceIndex = 0;

    private bool trackSwitching;

    private void Awake()
    {
        Instance = this;
    }
    public void SwitchTrack(AudioClip clip)
    {
        if(trackSwitching)
            currentSourceIndex = currentSourceIndex + 1 >= sources.Length ? 0 : currentSourceIndex + 1;

        StopAllCoroutines();
        StartCoroutine(FadeTrack(clip));
    }
    public void SetOverlayTrack(bool on, AudioClip clip = null)
    {
        StartCoroutine(FadeOverlayTrack(on, clip));
    }
    private IEnumerator FadeOverlayTrack(bool on, AudioClip clip)
    {
        if (on)
        {
            overlayLayer.clip = clip;
            while (overlayLayer.volume < 1)
            {
                overlayLayer.volume += Time.deltaTime / fadeLength;
                yield return null;
            }
            overlayLayer.volume = 1;
        }
        else
        {
            while (overlayLayer.volume > 0)
            {
                overlayLayer.volume -= Time.deltaTime / fadeLength;
                yield return null;
            }
            overlayLayer.volume = 0;
            overlayLayer.clip = null;
        }
    }
    private IEnumerator FadeTrack(AudioClip clip)
    {
        if (clip == sources[currentSourceIndex].clip)
            yield break;

        trackSwitching = true;

        int nextSourceIndex = currentSourceIndex + 1 >= sources.Length ? 0 : currentSourceIndex + 1;

        AudioSource source = sources[currentSourceIndex];
        AudioSource nextSource = sources[nextSourceIndex];

        nextSource.clip = clip;

        nextSource.volume = 0;

        while (source.volume > 0)
        {
            float volumeValue = Time.deltaTime / fadeLength;
            nextSource.volume += volumeValue;
            source.volume -= volumeValue;
            yield return null;
        }

        source.volume = 0;
        nextSource.volume = 1;

        currentSourceIndex = nextSourceIndex;
        trackSwitching = false;
    }
}