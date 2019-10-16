using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LifeBar : MonoBehaviour {
    public Image barreVie;
GameObject barreDeVieObject ;
    void Start () {
       
        barreDeVieObject = GameObject.FindGameObjectWithTag("viePleine");
        barreVie = barreDeVieObject.GetComponent<Image>();
       
    }
	void Update () {     
    }
    
    public void UpdateLifeBar(float damage)
    {
        if (barreVie.fillAmount==0.2f)
            barreVie.color= new Color32(255,0,0,0);

        float fillAmountDamage = (damage / 100);
        barreVie.fillAmount = fillAmountDamage;
        
    }
}
