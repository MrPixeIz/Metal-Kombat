﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesHitPointManager : MonoBehaviour {

    public GameObject lifeBar;
    public GameObject enemieNPC;
    private float HitPoint = 100;
    private float MaxHitPoint = 100;
    private float playerDamage = 1;

	// Use this for initialization
	void Start () {
    }
   
    public void ModifyHealthWithValue(float deltaModifier)
    {
        HitPoint -= deltaModifier;
        print("hitpoint " + HitPoint);
        //Possibilite de ne pas entrer dans le if currentLife == 0, imprecision float
        if (HitPoint <= 0)
        {
            HitPoint = 0;
           DieEvent();
        }
        SetLifeBarColor();
    }

    private void SetLifeBarColor()
    {
        float fillAmountPercent = (HitPoint / MaxHitPoint);
        print("fillAmountPercent " + fillAmountPercent);
        lifeBar.transform.localScale = new Vector3(fillAmountPercent, 0.1f, 0.01f);
    }
    void DieEvent()
    {
        Object.Destroy(enemieNPC);
    }
}