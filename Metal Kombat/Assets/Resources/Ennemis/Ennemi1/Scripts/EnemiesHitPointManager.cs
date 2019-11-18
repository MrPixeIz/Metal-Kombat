using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesHitPointManager : MonoBehaviour {

    public GameObject lifeBar;
    public GameObject enemieNPC;
    private float HitPoint = 100;
    private float MaxHitPoint = 100;
    private float playerDamage = 1;
    GameObject NPCPosition;
    GameObject instance;
    public GameObject lightning;
	// Use this for initialization
	void Start () {
       // NPCPosition = 
    }
   
    public void ModifyHealthWithValue(float deltaModifier)
    {
        HitPoint -= deltaModifier;
        print("hitpoint " + HitPoint);
        //Possibilite de ne pas entrer dans le if currentLife == 0, imprecision float
        print(enemieNPC.transform.position);
        instance = Instantiate(lightning, enemieNPC.transform.position, enemieNPC.transform.rotation) as GameObject;
        
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
