using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public FloatValue damage;

    private bool canDamage = true; 
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        ApplyDamageToPlayer(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        ApplyDamageToPlayer(other);
    }

    private void ApplyDamageToPlayer(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null && canDamage)
            {
                canDamage = false;
                other.gameObject.GetComponent<Player>().Hit(damage.initValue);
                StartCoroutine(DamageDelay());
            }
        }
    }
    
    private IEnumerator DamageDelay()
    {
        yield return new WaitForSeconds(1.5f);
        canDamage = true;
    }
}
