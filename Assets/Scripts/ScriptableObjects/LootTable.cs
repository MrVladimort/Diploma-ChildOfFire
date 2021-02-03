using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot
{
    public PowerupTrigger item;
    public int lootChance;
}

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    public List<Loot> loots;

    public PowerupTrigger GetLoot()
    {
        int cumProb = 0;
        int currentProb = Random.Range(0, 100);

        foreach (var loot in loots)
        {
            cumProb += loot.lootChance;
            if (currentProb <= cumProb)
            {
                return loot.item;
            }
        }
        
        return null;
    }
}
