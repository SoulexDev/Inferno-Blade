using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipTrigger : MonoBehaviour
{
    [SerializeField] private TipQueue[] tipQueues;
    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        if (other.CompareTag("Player"))
        {
            Player.Instance.tipMaster.CancelQueue();
            for (int i = 0; i < tipQueues.Length; i++)
            {
                Player.Instance.tipMaster.AddTipToQueue(tipQueues[i]);
            }
            triggered = true;
        }
    }
}