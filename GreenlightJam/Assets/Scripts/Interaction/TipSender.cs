using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipSender : MonoBehaviour
{
    [SerializeField] private TipQueue[] tipQueues;
    public void SendTip()
    {
        Player.Instance.tipMaster.CancelQueue();
        for (int i = 0; i < tipQueues.Length; i++)
        {
            Player.Instance.tipMaster.AddTipToQueue(tipQueues[i]);
        }
    }
}