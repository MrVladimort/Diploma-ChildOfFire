using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : InteractableTrigger
{
    public Item item;
    public Inventory playerInventory;
    public Signal getItem;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        dialogue.GetSentences().AddFirst("You've found some treasure. " + item.itemDescription);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void Interact()
    {
        playerInventory.AddItem(item);
        playerInventory.currentItem = item;
        getItem.Raise();
        DisableInteract();
        Destroy(gameObject);
    }
}