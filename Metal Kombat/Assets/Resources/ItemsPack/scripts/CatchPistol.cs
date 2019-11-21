using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPistol : MonoBehaviour
{
    private float timeBeforeDestroy = 0.6f;

    private MainPlayer mainPlayer;
    public AudioSource soundCatchGun;

    void Start()
    {      
        mainPlayer = GetComponent<MainPlayer>();
        soundCatchGun = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.name == "Player")
        {
            soundCatchGun.Play();
            mainPlayer = other.gameObject.GetComponent<MainPlayer>();
            Destroy(gameObject, timeBeforeDestroy);
            mainPlayer.AddModeAttaque();   
        }

    }
}
