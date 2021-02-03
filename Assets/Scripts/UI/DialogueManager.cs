using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public Text nameText;
    public Text dialogueText;
    
    private Animator animator;
    private Queue<string> sentencesQueue = new Queue<string>();

    private bool isOpen;
    
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IsOpen, isOpen);
        
        if (Input.GetButtonDown("Interact") && isOpen)
        {
            NextSentence();
        }
    }
    
    public void StartDialogue(Dialogue dialogue)
    {
        isOpen = true;
        nameText.text = dialogue.npcName;
        sentencesQueue = new Queue<string>(dialogue.GetSentences());
        TypeSentence(sentencesQueue.Dequeue());
    }

    void TypeSentence(string sentence)
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentenceCo(sentence));
    }

    IEnumerator TypeSentenceCo(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void NextSentence()
    {
        if (sentencesQueue.Count > 0)
        {
            TypeSentence(sentencesQueue.Dequeue());
        }
        else
        {
            isOpen = false;
            sentencesQueue.Clear();
        }
    }
}