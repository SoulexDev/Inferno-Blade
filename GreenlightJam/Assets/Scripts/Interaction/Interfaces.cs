using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact();
}
public interface IPlayer
{
    public void Damage(float amount);
}
public interface IEnemy
{
    public bool Damage(float amount, Vector3 attackDirection);
}