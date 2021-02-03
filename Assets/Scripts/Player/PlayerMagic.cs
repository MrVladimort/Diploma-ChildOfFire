using System;
using UnityEngine;


public class PlayerMagic : MonoBehaviour
{
    [Space] [Header("MagicSystem")] public FloatValue magic;

    private void Start()
    {
        magic.RuntimeValue = magic.initValue;
    }
    
    public bool CheckMagic(float magicPointsNeeded = 1)
    {
        return magic.RuntimeValue - magicPointsNeeded >= 0;
    }
    
    public void SpendMagicPower(float magicPoints)
    {
        if (magic.RuntimeValue - magicPoints >= 0) magic.RuntimeValue -= magicPoints;
        else throw new Exception("Dont have enough magic power");
    }

    public void RestoreMagicPower(float magicPoints)
    {
        if (magic.RuntimeValue + magicPoints > magic.initValue) magic.RuntimeValue = magic.initValue;
        else magic.RuntimeValue += magicPoints;
    }
}