using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsLoader : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string[] audioParameterNames;
    private void Awake()
    {
        foreach (var parameter in audioParameterNames)
        {
            SetAudio(parameter);
        }
    }
    void SetAudio(string parameterName)
    {
        float paramVal = PlayerPrefs.GetFloat(parameterName);
        float vol = 20 * Mathf.Log10(paramVal);
        if (paramVal == 0)
            vol = -80;
        mixer.SetFloat(parameterName, vol);
    }
}