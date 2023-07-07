using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public PlayerController controller;
    public PlayerStats stats;
    public Inventory inventory;
    public TipMaster tipMaster;
    public StatsDisplay statsDisplay;
    public Sword sword;

    [SerializeField] private GameObject deathMenu;

    public Vector3 spawnPos;
    public float xRot;

    private bool _dead;
    public bool dead { get { return _dead; } set { _dead = value; SetDeathMenuActivity(value); } }
    public bool paused;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(transform.parent);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(transform.parent);
        }
        
        Random.InitState(System.DateTime.Now.Millisecond);
    }
    public void ResetPlayer()
    {
        MusicManager.Instance.SwitchTrack(AudioManager.Instance.calmMusic);

        controller.camX = xRot;
        controller.camY = 0;
        controller.ResetForces();
        inventory.orbCount = 0;
        stats.ResetStats();
        transform.position = spawnPos;
    }
    private void SetDeathMenuActivity(bool value)
    {
        deathMenu.SetActive(value);
        if (value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}