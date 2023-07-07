using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    public void Restart()
    {
        Player.Instance.dead = false;
        ObjectPool.Instance.ResetPool();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadScene(int sceneIndex)
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (sceneIndex == 0)
        {
            SceneManager.MoveGameObjectToScene(Player.Instance.transform.parent.gameObject, SceneManager.GetActiveScene());
        }
        RankMaster.Instance.ExitScene();
        SceneManager.LoadScene(sceneIndex);
    }
    public void ResumeGame()
    {
        if (PauseSystem.Instance != null)
            PauseSystem.Instance.SetPausedState(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}