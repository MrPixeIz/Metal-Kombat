using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunBar : MonoBehaviour {

    private const float REDLIFEBARTHRESHOLD = 0.3f;
    public Image gunBar;
    public float currentNumber;
    private Personnage.OnDieEvent onDie;
    GameObject barreDeVieObject;
   
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
