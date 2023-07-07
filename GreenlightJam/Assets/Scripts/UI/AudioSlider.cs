using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private string parameterName;
    private void Awake()
    {
        slider.onValueChanged.AddListener(delegate { SetVolume(); });
    }
    public void SetVolume()
    {
        valueText.text = (slider.value * 100).ToString("f0");
        float vol = 20 * Mathf.Log10(slider.value);
        if (slider.value == 0)
            vol = -80;
        mixer.SetFloat(parameterName, vol);
    }
    private void OnEnable()
    {
        slider.value = PlayerPrefs.GetFloat(parameterName, 1);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(parameterName, slider.value);
        PlayerPrefs.Save();
        slider.value = PlayerPrefs.GetFloat(parameterName);
        SetVolume();
    }
}