using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    public static PauseSystem Instance;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject buttonsMenu;
    [SerializeField] private List<GameObject> menus;
    public delegate void Pause();
    public static event Pause OnPause;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPausedState(!Player.Instance.paused);
        }
    }
    public void SetPausedState(bool state)
    {
        if (Player.Instance.dead)
            return;
        OnPause?.Invoke();
        Player.Instance.paused = state;
        pauseMenu.SetActive(state);

        if (!state)
            menus.ForEach(m => m.SetActive(false));
        else
            buttonsMenu.SetActive(true);

        if (state)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}