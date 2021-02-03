using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerupTrigger : MonoBehaviour
{
    public Signal powerupSignal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("kek");

        if (other.CompareTag("Player"))
        {
            powerupSignal.Raise();
            PickupPowerup(other.GetComponent<Player>());
            Destroy(gameObject);
        }
    }

    protected abstract void PickupPowerup(Player player);
}
