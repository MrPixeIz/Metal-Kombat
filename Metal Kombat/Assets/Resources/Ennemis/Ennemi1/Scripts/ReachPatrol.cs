using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachPatrol : MonoBehaviour {

    public GameObject endPatrol;
    public GameObject startPatrol;

    public void OnTriggerEnter(Collider otherObject)
    {
        if (gameObject.name == startPatrol.name)
        {
            otherObject.GetComponent<EnnemiDetectionScript>().SetPatrol(endPatrol);
        }
        else
        {
            otherObject.GetComponent<EnnemiDetectionScript>().SetPatrol(startPatrol);
        }
    }
}
