using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedProjectile : Projectile
{
    public float hitAnimationDelay = 0.2f;
    public float fireDelay = 2f;
    
    private Animator anim;
    private static readonly int HitAnimTrigger = Animator.StringToHash("hit");
    private static readonly int MoveAnimTrigger = Animator.StringToHash("move");

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public override void Setup(Vector3 projectileDirection)
    {
        StartCoroutine(FireCo(projectileDirection));
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Door") || other.gameObject.CompareTag("Obstacles") ||
            other.gameObject.CompareTag("Platform"))
        {
            HitTarget();
        }
        else if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss")) && gameObject.CompareTag("Player Projectile"))
        {
            HitTarget();
        }
        else if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy Projectile"))
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        anim.SetTrigger(HitAnimTrigger);
        StartCoroutine(DestroyCo(hitAnimationDelay));
    }

    private IEnumerator FireCo(Vector3 projectileDirection)
    {
        yield return new WaitForSeconds(fireDelay);
        anim.SetTrigger(MoveAnimTrigger);
        base.Setup(projectileDirection);
    }
}
