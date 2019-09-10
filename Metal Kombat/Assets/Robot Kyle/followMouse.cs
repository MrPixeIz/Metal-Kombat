using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMouse : MonoBehaviour {

    public GameObject player;
    public Camera playerCamera;
    public float axisY = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetAxis("Mouse X") != 0)
        {
            player.transform.Rotate(0, Input.GetAxis("Mouse X") * 5, 0);
        }


        if (Input.GetAxis("Mouse Y") != 0 && axisY < 20 && axisY > -20)
        {
            axisY += Input.GetAxis("Mouse Y");
            playerCamera.transform.Rotate(-Input.GetAxis("Mouse Y") * 2, 0, 0);
            playerCamera.transform.Translate(0, -Input.GetAxis("Mouse Y") * 0.1f, 0);
        } else if (Input.GetAxis("Mouse Y") > 0 && axisY <= -20)
        {
            axisY += Input.GetAxis("Mouse Y");
            playerCamera.transform.Rotate(-Input.GetAxis("Mouse Y") * 2, 0, 0);
            playerCamera.transform.Translate(0, -Input.GetAxis("Mouse Y") * 0.1f, 0);
        } else if (Input.GetAxis("Mouse Y") < 0 && axisY >= 20)
        {
            axisY += Input.GetAxis("Mouse Y");
            playerCamera.transform.Rotate(-Input.GetAxis("Mouse Y") * 2, 0, 0);
            playerCamera.transform.Translate(0, -Input.GetAxis("Mouse Y") * 0.1f, 0);
        }
   
	}
}
