using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPistol : MonoBehaviour
{

    private MainPlayer mainPlayer;

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
            mainPlayer.AddModeAttaque();   
        }

    }
}
