using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightColor : MonoBehaviour {


    public Renderer renderer;

    // Use this for initialization
    void Start () {
        renderer = GetComponent<Renderer>();
    }
	
    public void ChangeLightColorSwitch()
    {
        renderer.material.SetColor("_EmissionColor", new Color32(255, 0, 0, 255));
        renderer.material.color = new Color32(255, 0, 0, 255);
    }
}
