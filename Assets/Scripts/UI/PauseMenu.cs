using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenuUI;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            {
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                paused = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                pauseMenuUI.SetActive(true);
                Time.timeScale = 0f;
                paused = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Back()
    {
        Time.timeScale = 1f;
        paused = false;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        paused = false;
        Application.Quit();
    }
}
