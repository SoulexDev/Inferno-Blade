using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStateSetter : MonoBehaviour
{
    public void SetCalm()
    {
        MusicManager.Instance.SwitchTrack(AudioManager.Instance.calmMusic);
    }
    public void SetCombat()
    {
        MusicManager.Instance.SwitchTrack(AudioManager.Instance.combatMusic);
    }
}