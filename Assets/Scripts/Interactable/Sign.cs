using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : InteractableTrigger
{
    protected override void Interact()
    {
        
    }

    protected override void StartDialogue()
    {
        dialogueManager.StartDialogue(dialogue);
    }
}
