using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionObject : MonoBehaviour {

    public int force = 1000;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        print("ON TRIGGER ENTER");
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name =="Player")
        {
            
        }
        
    }
}
