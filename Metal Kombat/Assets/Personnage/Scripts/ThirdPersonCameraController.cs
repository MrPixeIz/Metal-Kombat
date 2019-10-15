using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public float cameraMoveSpeed=120.0f;
    public GameObject cameraFollowObject;
    public float clampAngleX = 50.0f;
    public float clampAngleY = 90.0f;
    public float inputSensitivity = 150.0f;
    private float rotY = 0.0f;
    private float rotX = 0.0f;

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    void Update()
    {   

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotY += mouseX * inputSensitivity * Time.deltaTime;
        rotX+= mouseY * inputSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngleX, clampAngleX);
        //rotY = Mathf.Clamp(rotY, -clampAngleY, clampAngleY);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }

    private void LateUpdate()
    {
        CameraUpdater();
    }

    private void CameraUpdater()
    {
        Transform target = cameraFollowObject.transform;

        float step = cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
