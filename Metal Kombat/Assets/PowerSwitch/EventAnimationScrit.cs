using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnimationScrit : MonoBehaviour {

    public void EventEndAnimation()
    {
        GetComponentInParent<ControlerSwitch>().TurnLight();
    }
}
