using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public GameObject levelCompleteScreen;
    public PauseScript pause;
    public GameObject[] boss;

    void Update()
    {
        boss = GameObject.FindGameObjectsWithTag("Boss");
        if (boss.Length <= 0) LevelCompleted();
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")))
        {
            LevelCompleted();
        }
    }

    public void LevelCompleted()
    {
        levelCompleteScreen.SetActive(true);
        pause.paused = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }
}
