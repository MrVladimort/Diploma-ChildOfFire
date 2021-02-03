using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : PowerupTrigger
{
    public float healPoints = 1;
    
    protected override void PickupPowerup(Player player)
    {
        player.Heal(healPoints);
    }
}
