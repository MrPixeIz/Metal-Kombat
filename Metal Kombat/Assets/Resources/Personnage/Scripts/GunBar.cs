using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunBar : MonoBehaviour {

    public Image gunBar;
    public float currentNumber;
   
    public GunBar(Image image)
    {
        gunBar = image;
        currentNumber = 0;
    }
    
    public float ModifyGunBarWithValue(float deltaModifier)
    {
        currentNumber += deltaModifier;
        SetGunBarColor();
        return currentNumber;

    }
    private void SetGunBarColor()
    {
        float fillAmountPercent = (currentNumber / 100);
        gunBar.fillAmount = fillAmountPercent;
    }
}
