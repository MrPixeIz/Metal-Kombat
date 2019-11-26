using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachPatrol : EnnemiMovement {

    public GameObject endPatrol;
    public GameObject startPatrol;

    public void OnTriggerEnter(Collider otherObject)
    {
        if (gameObject.name == startPatrol.name)
        {
            otherObject.GetComponent<ReachPatrol>().SetPatrol(endPatrol);
        }
        else
        {
            otherObject.GetComponent<ReachPatrol>().SetPatrol(startPatrol);
        }
    }

    protected override void OnStart()
    {
    }

    protected override void OnUpdate()
    {
    }
}
