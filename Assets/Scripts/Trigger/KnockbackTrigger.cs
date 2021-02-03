using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackTrigger : MonoBehaviour
{
    public float thrust;
    public float knockTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                var hitForce = new Vector3(difference.x, 0, 0);

                if (other.gameObject.CompareTag("Enemy") && (gameObject.CompareTag("PlayerAttack") || gameObject.CompareTag("Player Projectile")))
                {
                    other.gameObject.GetComponent<Enemy>().Knock(hit, hitForce, knockTime);
                }
                else if (other.gameObject.CompareTag("Player") && (gameObject.CompareTag("EnemyAttack") || gameObject.CompareTag("Enemy Projectile")))
                {
                    other.gameObject.GetComponent<Player>().Knock(hit, hitForce, knockTime);
                }
            }
        }
    }
}