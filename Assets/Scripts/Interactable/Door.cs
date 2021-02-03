using System;
using UnityEngine;

public enum DoorType
{
    Key,
    Enemy,
    Button
}

public class Door : InteractableTrigger
{
    [Header("Door variables")] public DoorType doorType;
    public bool isOpened;
    public Inventory playerInventory;
    
    [Header("Positions")]
    public Transform openPosition;
    public Transform closedPosition;

    private MoveableObject move;

    protected override void Start()
    {
        base.Start();

        move = GetComponent<MoveableObject>();
    }

    protected override void Update()
    {
        base.Update();
        
        if (isOpened && !move.CheckDistanceToTarget(openPosition.position, 0.1f))
        {
            move.Move(move.GetDirectionToTarget(openPosition.position, false));
            DisableInteract();
        } else if (!isOpened && !move.CheckDistanceToTarget(closedPosition.position, 0.1f))
        {
            move.Move(move.GetDirectionToTarget(closedPosition.position, false));
            DisableInteract();
        }
        else
        {
            move.StopMoving();
            EnableInteract();
        }
    }

    public void Open()
    {
        isOpened = true;
    }

    public void Close()
    {
        isOpened = false;
    }

    protected override void Interact()
    {
        if (doorType == DoorType.Key)
        {
            //Does the player have a key?
            if (playerInventory.TakeKey())
            {
                dialogue.ClearAndAdd("Door is opening");
                //If so, then call the open method
                Open();
            }
        }
    }
}