using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrbStand : MonoBehaviour, IInteractable
{
    public UnityEvent acceptEvent;
    [SerializeField] private Transform orbSpawnPos;
    [SerializeField] private GameObject openerOrb;

    private bool used = false;
    public void Interact()
    {
        if (used)
            return;
        if (Player.Instance.inventory.AttemptUseOrb())
        {
            acceptEvent.Invoke();
            Instantiate(openerOrb, orbSpawnPos.position, Quaternion.identity).GetComponent<OpenerOrb>().interactable = false;
            used = true;
        }
    }
}