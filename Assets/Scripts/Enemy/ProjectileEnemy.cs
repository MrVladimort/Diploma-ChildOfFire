using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Enemy
{
    public Projectile projectile;
    public Transform startBulletPosition;

    protected override void AttackLogic()
    {
        base.AttackLogic();
        
        var instantiatedProjectile =
            Instantiate(projectile, startBulletPosition.transform.position, Quaternion.identity);
        instantiatedProjectile.Setup(move.direction);
    }
}