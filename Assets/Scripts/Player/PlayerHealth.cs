using UnityEngine;

public class PlayerHealth : LiveThing
{ 
    protected override void Start()
    {
        health.RuntimeValue = health.initValue;
        currentHealth = health.RuntimeValue;
    }
    
    public override void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage);

        health.RuntimeValue = currentHealth;
    }

    public override void RestoreHealth(float healthToRestore)
    {
        base.RestoreHealth(healthToRestore);

        health.RuntimeValue = currentHealth;
    }

    public override bool CheckDead()
    {
        return health.RuntimeValue > 0;
    }
}