using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public Text arrowText;
    public Text keyText;
    public Inventory playerInventory;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        arrowText.text = playerInventory.numberOfArrows.RuntimeValue.ToString();
        keyText.text = playerInventory.numberOfKeys.RuntimeValue.ToString();
    }
}
