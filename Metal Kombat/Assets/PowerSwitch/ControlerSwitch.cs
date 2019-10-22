using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlerSwitch : MonoBehaviour {

    private Animator animation;
    private AudioSource soundSwitch; 


    // Use this for initialization
    void Start () {

        animation = this.GetComponentInChildren<Animator>();
        soundSwitch = this.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        
    }

    void OnTriggerEnter()
    {
        animation.SetTrigger("TriggerSwitch");
        //soundSwitch.Play();


    }

    public void TurnLight()
    {
        print("cacou");
        soundSwitch.Play();
    }
}
