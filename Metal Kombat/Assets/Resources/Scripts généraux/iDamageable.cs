using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iDamageable {

    int DamageAmount { get;}
    void TakeDammageInt(int damageAmount);
    	
}
