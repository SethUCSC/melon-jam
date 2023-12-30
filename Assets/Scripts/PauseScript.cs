using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PauseScript : MonoBehaviour
{
    [Header("Pause Menu UI")]
    public GameObject pausemenu;
    public GameObject player;
    public GameObject cam;
    public PlayerHealth health;
    // public GameObject settingsMenu;

    public bool paused = false;

    void Update()
    {
        if(Input.GetKeyDown("escape") && !health.playerDead)
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pausing();
            }
        }

    }

    public void Pausing()
    {
        paused = true;
        pausemenu.SetActive(true);
        AudioListener.pause = true;
        // settingsMenu.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        paused = false;
        // player.SetActive(true);
        // cam.SetActive(true);
        pausemenu.SetActive(false);
        AudioListener.pause = false;
        // settingsMenu.SetActive(false);
        Time.timeScale = 1f;    
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Settings()
    {
        paused = false;
        // settingsMenu.SetActive(true);
        pausemenu.SetActive(false);
        Time.timeScale = 0f;    
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Restart()
    {
        SceneManager.LoadScene("MovementTest");
    }
}
