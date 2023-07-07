using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffectPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    public void Init(AudioClip[] clips)
    {
        for (int i = 0; i < clips.Length; i++)
        {
            AudioManager.Instance.PlayAudioOnSource(source, clips[i]);
        }
    }
}