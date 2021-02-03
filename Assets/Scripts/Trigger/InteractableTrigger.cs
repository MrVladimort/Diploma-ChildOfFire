using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractableTrigger : MonoBehaviour
{
    [Space] [Header("Help props")]
    [SerializeField] GameObject helpTextBoxObject;
    public string helpText;
    private TextMesh helpTextObject;

    protected DialogueManager dialogueManager;
    public Dialogue dialogue;
    
    [Space] [Header("UI bools")] 
    public bool useHelpText;
    public bool useDialog;
    
    [Space] [Header("Context clue signals")]
    public Signal contextClueEnable;
    public Signal contextClueDisable;

    protected bool isInteractable = true;

    private bool playerInRange;
    private bool playerInteracting;
    
    protected virtual void Start()
    {
        dialogueManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>().GetDialogueManager();
        
        if (useHelpText)
        {
            helpTextObject = helpTextBoxObject.GetComponent<TextMesh>();
            helpTextObject.text = helpText;
        }
    }

    protected virtual void Update()
    {
        if (isInteractable && !playerInteracting && playerInRange && Input.GetButtonDown("Interact"))
        {
            playerInteracting = true;
            DisableHelpClues();
            Interact();
            if (useDialog) StartDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player") && isInteractable)
        {
            EnableHelpClues();
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            DisableHelpClues();
            playerInRange = false;
            playerInteracting = false;
        }
    }
    
    protected void EnableInteract()
    {
        isInteractable = true;
    }
    
    protected void DisableInteract()
    {
        isInteractable = false;
    }

    protected void EnableHelpClues()
    {
        if (useHelpText) helpTextBoxObject.SetActive(true);
        contextClueEnable.Raise();
    }
    
    protected void DisableHelpClues()
    {
        if (useHelpText) helpTextBoxObject.SetActive(false);
        contextClueDisable.Raise();
    }

    protected abstract void Interact();

    protected virtual void StartDialogue()
    {
        dialogueManager.StartDialogue(dialogue);
    }
}
