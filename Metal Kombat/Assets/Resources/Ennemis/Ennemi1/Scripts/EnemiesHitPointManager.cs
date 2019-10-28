using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesHitPointManager : MonoBehaviour {

    GameObject lifeBar;
    GameObject enemieNPC;
    public float HitPoint = 100;
    public float MaxHitPoint = 100;
    public float playerDamage = 1;

	// Use this for initialization
	void Start () {
        lifeBar = GameObject.Find("LifeBar");
        enemieNPC = GameObject.Find("vanguard_t_choonyung@Holding Idle (1)");
    }
    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetAxis("Fire3") != 0)
    //    {
    //        ModifyHealthWithValue(1);
    //    }
    //}
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
        lifeBar.transform.localScale = new Vector3(fillAmountPercent, 0.2222222f, 0.01f);
    }
    void DieEvent()
    {
        Object.Destroy(enemieNPC);
    }
}
