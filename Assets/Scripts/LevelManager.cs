using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private Animator animator;

    private string sceneNameToLoad;
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeToLevel(string sceneToLoad)
    {
        sceneNameToLoad = sceneToLoad;
        animator.SetTrigger(FadeOut);
    }
    
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneNameToLoad);
    }
}
