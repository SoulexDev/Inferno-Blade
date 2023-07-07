using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioClip lastClip;
    [SerializeField] private AudioSource globalSource;
    [SerializeField] private int clipStackingAmount = 3;
    [SerializeField] private float clipCooldown = 0.05f;
    private int currentClipStack;
    private float timer;
    private void Awake()
    {
        Instance = this;
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    [Header("Effects")]
    public AudioClip grappleBlue;
    public AudioClip grappleRed;
    public AudioClip grappleRelease;

    public AudioClip[] swordSwings;

    public AudioClip playerLand;
    public AudioClip playerDash;

    public AudioClip bloodExplosion;
    public AudioClip doorOpen;

    //[Header("Voices")]


    [Header("Music")]
    public AudioClip calmMusic;
    public AudioClip combatMusic;

    private void Update()
    {
        timer -= Time.deltaTime;
        timer = Mathf.Clamp(timer, 0, timer);
        if(timer <= 0)
        {
            currentClipStack = 0;
            timer = 0;
        }
    }
    public void PlayAudioOnSource(AudioSource source, AudioClip clip)
    {
        if(clip == lastClip && currentClipStack >= clipStackingAmount)
        {
            timer = clipCooldown;
            return;
        }

        currentClipStack++;
        source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(clip);
        lastClip = clip;
    }
    public void PlayAudioOnGlobalSource(AudioClip clip, bool oneShot = true)
    {
        globalSource.pitch = Random.Range(0.8f, 1.2f);
        if (oneShot)
            globalSource.PlayOneShot(clip);
        else
        {
            globalSource.clip = clip;
            globalSource.Play();
        }
    }
}