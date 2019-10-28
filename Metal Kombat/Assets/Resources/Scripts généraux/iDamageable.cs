using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iDamageable {

    int DamageAmount { get; set; }
    void TakeDammageInt(int damageAmount);
    	
}
