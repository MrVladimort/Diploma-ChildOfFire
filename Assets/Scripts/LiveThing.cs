using System;
using UnityEngine;

public class LiveThing : MonoBehaviour
{
    [Space] [Header("HealthSystem")] public FloatValue health;
    public float currentHealth;

    protected virtual void Start()
    {
        currentHealth = health.initValue;
        health.RuntimeValue = currentHealth;
    }

    public virtual void ApplyDamage(float damage)
    {
        if (currentHealth - damage < 0) currentHealth = 0;
        else currentHealth -= damage;
    }

    public virtual void RestoreHealth(float healthToRestore)
    {
        if (currentHealth + healthToRestore > health.initValue) currentHealth = health.initValue;
        else currentHealth += healthToRestore;
    }

    public virtual bool CheckDead()
    {
        return currentHealth > 0;
    }

    public virtual bool CheckFullHealth()
    {
        return currentHealth == health.initValue;
    }
}