using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }
    }

    public void resumeGame()
    {
        pauseMenu.SetActive(false);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
