using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchMedKit : MonoBehaviour {

    private MainPlayer mainPlayer;
    private float ValueHealthKit = 30;

    void Start()
    {
        mainPlayer = GetComponent<MainPlayer>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.name == "Player")
        {
            mainPlayer = other.gameObject.GetComponent<MainPlayer>();
            Destroy(gameObject);
            mainPlayer.IncreaseLife(ValueHealthKit);
        }

    }
}
