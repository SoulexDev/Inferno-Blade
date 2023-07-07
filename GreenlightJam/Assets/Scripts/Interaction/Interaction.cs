using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            if(Physics.Raycast(ray, out RaycastHit hit, 3, ~LayerMask.GetMask("Ignore Player", "Player", "Ignore Raycast")))
            {
                if (hit.transform.TryGetComponent(out IInteractable interactable))
                    interactable.Interact();
            }
        }
    }
}