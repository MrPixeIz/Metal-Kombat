using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPistol : MonoBehaviour {

    private int speedRotation = 4;
	
	void Start () {
		
	}
	
	void Update () {

        RotationGun();
    }

    private void RotationGun()
    {

        Vector3 rotationMove = new Vector3(0, speedRotation, 0);

        gameObject.transform.Rotate(rotationMove);
    } 
}
