using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartTrigger : MonoBehaviour
{
    private bool triggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            RankMaster.Instance.StartLevel();
            triggered = true;
        }
    }
}