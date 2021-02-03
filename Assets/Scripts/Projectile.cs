using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class Projectile : MoveableObject
{
    public virtual void Setup(Vector3 projectileDirection)
    {
        transform.rotation = Quaternion.Euler(projectileDirection);
        Move(projectileDirection);
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Door") || other.gameObject.CompareTag("Obstacles") ||
            other.gameObject.CompareTag("Platform"))
        {
            StartCoroutine(DestroyCo(1f));
        }
        else if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss")) && gameObject.CompareTag("Player Projectile"))
        {
            StartCoroutine(DestroyCo(0.1f));
        }
        else if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy Projectile"))
        {
            StartCoroutine(DestroyCo(0.1f));
        }
    }

    protected IEnumerator DestroyCo(float destroyDelay)
    {
        Move(Vector2.zero);
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}