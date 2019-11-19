using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchMedKit : MonoBehaviour {

    private float timeBeforeDestroy = 0.6f;
    private float ValueHealthKit = 30;

    private MainPlayer mainPlayer;
    public AudioSource soundCatchMedKit;

    void Start()
    {
        mainPlayer = GetComponent<MainPlayer>();
        soundCatchMedKit = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.name == "Player")
        {
            soundCatchMedKit.Play();
            mainPlayer = other.gameObject.GetComponent<MainPlayer>();
            Destroy(gameObject, timeBeforeDestroy);
            mainPlayer.IncreaseLife(ValueHealthKit);
        }

    }
}
