
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableTrigger
{
    [Space][Header("Chest props")]
    public Item contents;
    public Inventory playerInventory;
    public bool isOpen;

    private Animator animator;

    private static readonly int IsOpenAnimatorMapping = Animator.StringToHash("isOpen");

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        animator = GetComponent<Animator>();
        
        dialogue.GetSentences().AddFirst("You've found some treasure. " + contents.itemDescription);
    }

    private void OpenChest()
    {
        playerInventory.AddItem(contents);
        playerInventory.currentItem = contents;
        isOpen = true;
        
        DisableInteract();
    }

    protected override void Interact()
    {
        if (!isOpen)
        {
            OpenChest();
        }
        else
        {
            dialogue.ClearAndAdd("Nothing to do here, chest is empty");
        }
        
        animator.SetBool(IsOpenAnimatorMapping, isOpen);
    }
}
