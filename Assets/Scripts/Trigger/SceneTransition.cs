using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public PlayerPosition playerStorage;
    
    
    private LevelManager _levelManager;
    
    private void Start()
    {
        _levelManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>().GetLevelManager();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerStorage.position.RuntimeValue = playerPosition;
            playerStorage.scene.RuntimeValue = sceneToLoad;
            _levelManager.FadeToLevel(sceneToLoad);
        }
    }
}
