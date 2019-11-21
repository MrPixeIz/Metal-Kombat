using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    private const float REDLIFEBARTHRESHOLD = 0.3f;
    public Image barreVie;
    private float maxLife;
    private float currentLife;
    private Personnage.OnDieEvent onDie;
    GameObject barreDeVieObject;

    public LifeBar(Image image, float inMaxlife)
    {
        barreVie = image;
        maxLife = inMaxlife;
        currentLife = maxLife;

    }
    public LifeBar(float inMaxlife)
    {
        
        maxLife = inMaxlife;
        currentLife = maxLife;

    }

    public void SetOnDieListenner(Personnage.OnDieEvent onDieEvent)
    {
        onDie = onDieEvent;

    }
    public void ModifyHealthWithValue(float deltaModifier)
    {
        currentLife += deltaModifier;
        //Possibilite de ne pas entrer dans le if currentLife == 0, imprecision float
        if (currentLife <= 0)
        {
            currentLife = 0;
            onDie.OnDieEvent();
        }
        SetLifeBarColor();

    }
    public void AdjusteHealthBar(float vie)
    {
        
        currentLife += vie;
        if (currentLife > 100)
        {
            currentLife = 100;
        }
        SetLifeBarColor();

    }

    private void SetLifeBarColor()
    {
        float fillAmountPercent = (currentLife / maxLife);
        barreVie.fillAmount = fillAmountPercent;
        
        if (fillAmountPercent <= REDLIFEBARTHRESHOLD)
        {
            barreVie.color = new Color32(255, 0, 0, 255);
        }
        else
        {
            barreVie.color = new Color32(255, 255, 255, 255);
        }
    }
}
