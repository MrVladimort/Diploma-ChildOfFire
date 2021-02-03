using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    public Image barStart;
    public Image[] bars;
    public Image barEnd;

    public FloatValue barValue;

    public void Start()
    {
        UpdateBars();
    }

    private void LateUpdate()
    {
        UpdateBars();
    }

    public void UpdateBars()
    {
        if (barValue.RuntimeValue > 0)
            barStart.gameObject.SetActive(true);
        else
            barStart.gameObject.SetActive(false);

        for (int i = 0; i < barValue.initValue - 2; i++)
        {
            if (i < barValue.RuntimeValue - 1)
                bars[i].gameObject.SetActive(true);
            else
                bars[i].gameObject.SetActive(false);
        }

        if (barValue.initValue == barValue.RuntimeValue)
            barEnd.gameObject.SetActive(true);
        else
            barEnd.gameObject.SetActive(false);
        
    }
}