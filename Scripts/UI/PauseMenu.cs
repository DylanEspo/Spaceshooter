using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject controlsMenu;
    public GameObject displayMenu;
    public GameObject audioMenu;

    public static bool isPaused = true;

    void Awake()
    {
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        displayMenu.SetActive(false);
        audioMenu.SetActive(false);
        pauseMenu.SetActive(false);

    }

    // Update is called once per frame
    public void PauseGame()
    {
        if(optionsMenu.active || controlsMenu.active || displayMenu.active || audioMenu.active)
        {
            return;
        }
        else
        {
            Debug.Log("PauseMenuActivated");
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OptionsMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void DisplayMenu()
    {
        optionsMenu.SetActive(false);
        displayMenu.SetActive(true);
    }

    public void AudioMenu()
    {
        optionsMenu.SetActive(false);
        audioMenu.SetActive(true);
    }

    public void ControlsMenu()
    {
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void ReturnToOptions()
    {
        displayMenu.SetActive(false);
        audioMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ExitOptionsMenu()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ExitControlsMenu()
    {
        controlsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        SceneManager.LoadScene("MainMenu");
    }
}
