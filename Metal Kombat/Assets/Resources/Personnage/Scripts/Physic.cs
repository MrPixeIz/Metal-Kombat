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
    protected bool isCrouched;
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
            {// Vector3.Angle(Vector3.up, hitNormal) >= SLOPELIMIT) {                
                float slideFriction = 0f;
                velocity.y = 0;
                velocity.x = ((1f - hitNormal.y) * hitNormal.x * (1.3f - slideFriction)) * gravity * Time.deltaTime * 25;
                velocity.z = ((1f - hitNormal.y) * hitNormal.z * (1.3f - slideFriction)) * gravity * Time.deltaTime * 25;
                /* player.ChangeValueMoveVectorX(((1f - hitNormal.y) * hitNormal.x * (1.3f - slideFriction)) * gravity * Time.deltaTime * 5);
                 player.ChangeValueMoveVectorZ(((1f - hitNormal.y) * hitNormal.z * (1.3f - slideFriction)) * gravity * Time.deltaTime * 5);*/
            }
        } /*else if (hitNormal == Vector3.zero ) {
            //&& player.Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Fall A Loop"
            MoveCharacterWhenStuck(10);
        }*/
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

    /*void OnCollisionEnter(Collision collision) {
        print("COLLISION ENTER");
        
        hitNormal = collision.contacts[0].normal;
    }*/


    /* void OnControllerColliderHit(ControllerColliderHit hit)
     {
         hitNormal = hit.normal;
     }*/

    private bool IsInContactWithSlope()
    {
        RaycastHit hit;

        bool touchGround = Physics.SphereCast(transform.position + new Vector3(0, 2.5f, 0), 1.75f, Vector3.down, out hit, 1f);

        if (touchGround)
        {
            hitNormal = hit.normal;
            Debug.DrawLine(hit.point, (hit.point + hit.normal * 6), Color.cyan);
            
            if (Vector3.Angle(Vector3.up, hitNormal) <= SLOPELIMIT)
            {
                if (!isCrouched)
                    isGrounded = true;

            }
        }

        return touchGround;
    }
    /*void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
       
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + new Vector3(0, 1.5f, 0), 1.75f);
    }*/



    /*void MoveCharacterWhenStuck(float force)
    {
        print("MoveCharacterWhenStuck");
        player.ChangeValueMoveVectorX(0);
        player.ChangeValueMoveVectorZ(0);

        player.MoveVector += transform.forward * force * Time.deltaTime * Mathf.Abs(10);

        player.MoveVector += transform.forward * force * Time.deltaTime * Mathf.Abs(10);

        player.MoveVector += transform.forward * force * Time.deltaTime * Mathf.Abs(-20);

        player.MoveVector += transform.forward * force * Time.deltaTime * Mathf.Abs(-20);


    }*/




    bool GroundCheck()
    {
        RaycastHit hit;
        float distance = 2.2f;
        Vector3 startPosition;
        Vector3 dir = new Vector3(0, -distance, 0);
        if (!isCrouched)
        {
            startPosition = transform.position;
        }
        else 
        {
            startPosition = transform.position + new Vector3(0, 1.8f, 0);
        }

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