﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouvement : MonoBehaviour {

    // Use this for initialization

    private CharacterController characterController;
	void Start () {
        characterController = GetComponent<CharacterController>();

    }
	
	// Update is called once per frame
	void Update () {

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        Vector3 mouvement = new Vector3(horizontal,0,vertical);
        Vector3 ajustedMouvement = transform.TransformDirection(mouvement);
        characterController.SimpleMove(ajustedMouvement * Time.deltaTime * 500);


    }
}