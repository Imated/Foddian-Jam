using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused;
    
    [SerializeField] GameObject pauseMenu;
    [SerializeField] UIMoveY uiMover;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiMover.Tween();
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            IsPaused = Time.timeScale == 0;
        }
    }

    public void resumeGame()
    {
        uiMover.Tween();
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        IsPaused = Time.timeScale == 0;
    }

    public void mainMenu()
    {
        TransitionManager.Instance.LoadSceneFade("Main Menu");
    }

    public void Quit()
    {
        TransitionManager.Instance.Exit();
    }
}
