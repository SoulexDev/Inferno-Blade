using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI timeDisplay;
    [SerializeField] private TextMeshProUGUI killDisplay;

    private int _killAmount;
    public int killAmount { get { return _killAmount; } set { _killAmount = value; killDisplay.text = "Kills: " + _killAmount.ToString(); } }
}