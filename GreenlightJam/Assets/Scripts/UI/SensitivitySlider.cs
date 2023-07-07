using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        slider.onValueChanged.AddListener(delegate { SetSensitivity(); });
    }
    public void SetSensitivity()
    {
        valueText.text = slider.value.ToString("f1");
    }
    private void OnEnable()
    {
        slider.value = PlayerPrefs.GetFloat("Sensitivity", 2.5f);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat("Sensitivity", slider.value);
        PlayerPrefs.Save();
        slider.value = PlayerPrefs.GetFloat("Sensitivity");
        SetSensitivity();
    }
}