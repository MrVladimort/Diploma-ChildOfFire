using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject howToPanel;
    public GameObject chooseLevelPanel;

    public void ShowHowToPanel()
    {
        menuPanel.SetActive(false);
        howToPanel.SetActive(true);
    }

    public void ShowMenuPanel()
    {
        menuPanel.SetActive(true);
        howToPanel.SetActive(false);
        chooseLevelPanel.SetActive(false);
    }
    
    public void ShowChoosePanel()
    {
        chooseLevelPanel.SetActive(true);
        menuPanel.SetActive(false);
    }
    
    public void StartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    
    public void LoadLevel1(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void LoadLevel2(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    
    public void LoadLevel3(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }
    
    public void LoadLevel4(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
    }
    
    public void LoadLevel5(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 5);
    }
    
    public void LoadLevel6(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 6);
    }
    public void LoadBossLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 7);
    }
    
    public void LoadDebugLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 9);
    }
}