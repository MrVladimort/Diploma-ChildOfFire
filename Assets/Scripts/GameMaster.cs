using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private Player player;
    private UIManager uiManager;
    private LevelManager levelManager;
    private SoundManager soundManager;

    private static GameMaster _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        levelManager = GetComponent<LevelManager>();
    }
    
    public IEnumerator RestartLevel()
    {
        levelManager.FadeToLevel(SceneManager.GetActiveScene().name);
        yield return new WaitForSeconds(2.5f);
    }

    public IEnumerator ExitToMainMenu()
    {
        levelManager.FadeToLevel("Scenes/StartMenu");
        yield return new WaitForSeconds(2.5f);
        Destroy(_instance);
    }

    public Player GetPlayer()
    {
        return player;
    }

    public LevelManager GetLevelManager()
    {
        return levelManager;
    }
    
    public DialogueManager GetDialogueManager()
    {
        return uiManager.dialogueManager;
    }
    
    public SoundManager GetSoundManager()
    {
        return soundManager;
    }
}