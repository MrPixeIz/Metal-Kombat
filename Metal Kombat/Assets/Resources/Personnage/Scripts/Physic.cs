using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Physic : MonoBehaviour
{
    private float gravity = 65.0f;
    private const float SLOPELIMIT = 50;
    protected Vector3 velocity = new Vector3(0, 0, 0);
    protected bool isGrounded;
    protected Vector3 hitNormal;
    protected bool canslide =true;

    void Start()
    {
    }
    void Update()
    {
        isGrounded = GroundCheck();
        ApplyGravity();
        ApplySlide();
        ApplyVelocity();
        OnUpdate();
    }

    protected abstract void ApplyVelocity();

    protected abstract void OnUpdate();

    private void ApplySlide()
    {
        IsInContactWithSlope();
        if (hitNormal != Vector3.zero)
        {
            if (!IsGoingUp() && !isGrounded)
            {               
                float slideFriction = 0f;
                velocity.y = 0;
                velocity.x = ((1f - hitNormal.y) * hitNormal.x * (1.3f - slideFriction)) * gravity * Time.deltaTime * 25;
                velocity.z = ((1f - hitNormal.y) * hitNormal.z * (1.3f - slideFriction)) * gravity * Time.deltaTime * 25;    
            }
        } 
        hitNormal = Vector3.zero;
    }

    private bool IsGoingUp()
    {
        return velocity.y > 0;
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else
        {
            velocity.x = 0;
            velocity.z = 0;
            if (velocity.y < 0)
            {
                velocity.y = 0;
            }
        }

    }

    private bool IsInContactWithSlope()
    {
        RaycastHit hit;
        bool touchGround = Physics.SphereCast(transform.position + new Vector3(0, 2.5f, 0), 1.75f, Vector3.down, out hit, 1f);
        if (touchGround)
        {
            hitNormal = hit.normal;      
            if (Vector3.Angle(Vector3.up, hitNormal) <= SLOPELIMIT)
            {          
                    isGrounded = true;
            }
        }
        return touchGround;
    }
    
    bool GroundCheck()
    {
        RaycastHit hit;
        float distance = 2.2f;
        Vector3 startPosition;
        Vector3 dir = new Vector3(0, -distance, 0);  
        startPosition = transform.position;
        startPosition.y += 2f;
        Debug.DrawRay(startPosition, dir, Color.red, 1);
        bool isGroundeReturnValue;
        if (Physics.Raycast(startPosition, dir, out hit, distance))
        {
            isGroundeReturnValue = true;
        }
        else
        {
            isGroundeReturnValue = false;
        }
        return isGroundeReturnValue;
    }
}