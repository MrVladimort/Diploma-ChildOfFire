using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject exitMenu;
    public Text pauseText;
    public GameObject resumeButton;
    public Text exitBackButtonText;

    private GameMaster gameMaster;
    private bool canUnpause = true;
    private bool deathPause = false;

    void Start()
    {
        gameMaster = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        pauseMenu.SetActive(false);
        exitMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && Time.timeScale == 1)
        {
            ShowPausePanel();
        } else if (Input.GetButtonDown("Pause") && Time.timeScale == 0 && canUnpause)
            Resume();
    }

    public void ShowPausePanel()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        exitMenu.SetActive(false);
    }

    public void ShowExitPanel()
    {
        pauseMenu.SetActive(false);
        exitMenu.SetActive(true);
    }

    public void ShowDeathPanel()
    {
        if (!deathPause)
        {
            canUnpause = false;
            deathPause = true;
            resumeButton.SetActive(false);
            pauseText.text = "You Died";
            pauseText.color = Color.red;
            exitBackButtonText.text = "Back To Death Menu";
            ShowPausePanel();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        exitMenu.SetActive(false);
    }

    public void RestartLevel()
    {
        StartCoroutine( gameMaster.RestartLevel());
        Resume();
    }

    public void ExitToMainMenu(){
        StartCoroutine( gameMaster.ExitToMainMenu());
        Resume();
    }

    public void Exit()
    {
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
