using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPowerup : PowerupTrigger
{
    public Item item;
    
    protected override void PickupPowerup(Player player)
    {
        player.inventory.AddItem(item);
    }
}
