using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private void Start()
    {
        if(Player.Instance == null)
        {
            Instantiate(player);
        }
    }
}