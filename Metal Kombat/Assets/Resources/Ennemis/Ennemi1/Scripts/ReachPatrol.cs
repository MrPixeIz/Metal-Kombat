using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachPatrol : MonoBehaviour
{

    public GameObject endPatrol;
    public GameObject startPatrol;



    public void OnTriggerEnter(Collider otherObject)
    {


        if (gameObject.name == startPatrol.name )
        {
            if (otherObject.name == "vanguard_t_choonyung@Holding Idle (1)")
                otherObject.GetComponent<EnnemiDetectionScript>().SetPatrol(endPatrol);

            

        }
        else if (gameObject.name == endPatrol.name)
        {
            if (otherObject.name == "vanguard_t_choonyung@Holding Idle (1)")
                otherObject.GetComponent<EnnemiDetectionScript>().SetPatrol(startPatrol);
        }
    }


}
