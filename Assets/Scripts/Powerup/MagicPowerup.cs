using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPowerup : PowerupTrigger
{
    public float magicPoints = 1;
    
    protected override void PickupPowerup(Player player)
    {
        player.magic.RestoreMagicPower(magicPoints);
        player.playerMagicSignal.Raise();
    }
}
