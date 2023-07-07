using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenerOrb : MonoBehaviour, IInteractable
{
    public bool interactable = true;
    public void Interact()
    {
        if (!interactable)
            return;
        Player.Instance.inventory.orbCount++;
        Destroy(gameObject);
    }
}