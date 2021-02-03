using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    public Item currentItem;
    public Signal inventoryUpdateSignal;
    public List<Item> inventoryItems = new List<Item>();
    public FloatValue numberOfKeys;
    public FloatValue numberOfArrows;

    public void AddItem(Item itemToAdd)
    {
        if (itemToAdd.isKey)
        {
            numberOfKeys.RuntimeValue++;
        }
        else if (itemToAdd.isArrow)
        {
            numberOfArrows.RuntimeValue++;
        }
        else
        {
            inventoryItems.Add(itemToAdd);
        }
        
        inventoryUpdateSignal.Raise();
    }

    public bool TakeKey()
    {
        if (numberOfKeys.RuntimeValue > 0)
        {
            numberOfKeys.RuntimeValue--;
            inventoryUpdateSignal.Raise();
            return true;
        }
 
        return false;
    }

    public bool TakeArrow()
    {
        if (numberOfArrows.RuntimeValue > 0)
        {
            numberOfArrows.RuntimeValue--;
            inventoryUpdateSignal.Raise();
            return true;
        }
 
        return false;
    }
}