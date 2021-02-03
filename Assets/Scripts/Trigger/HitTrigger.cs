using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    public FloatValue damage;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Boss"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                if (other.gameObject.CompareTag("Enemy") && (gameObject.CompareTag("PlayerAttack") || gameObject.CompareTag("Player Projectile")))
                {
                    other.gameObject.GetComponent<Enemy>().Hit(damage.initValue);
                }
                else if (other.gameObject.CompareTag("Boss") && (gameObject.CompareTag("PlayerAttack") || gameObject.CompareTag("Player Projectile")))
                {
                    other.gameObject.GetComponent<Boss>().Hit(damage.initValue);
                }
                else if (other.gameObject.CompareTag("Player") && (gameObject.CompareTag("EnemyAttack") || gameObject.CompareTag("Enemy Projectile")))
                {
                    other.gameObject.GetComponent<Player>().Hit(damage.initValue);
                }
            }
        }
    }
}
