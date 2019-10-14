using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Physic : MonoBehaviour
{
    //public MainPlayer player;

    //public float desiredRotationSpeed;
   // public bool ikActive = false;

    private float gravity = 65.0f;
    private const float SLOPELIMIT = 50;
    protected Vector3 velocity = new Vector3(0, 0, 0);
    protected bool isGrounded;
    protected Vector3 hitNormal;
    //private float verticalVel = 0;


    public GameObject gun;
    void Start()
    {

       // player = GetComponent<MainPlayer>();


       // sounds = GetComponentInChildren<Sounds>();
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

    private void ApplySlide() {
        IsInContactWithSlope();
        if (hitNormal != Vector3.zero) {
            print(Vector3.Angle(Vector3.up, hitNormal));
            print(IsGoingUp());
            if (!IsGoingUp() &&  !isGrounded){// Vector3.Angle(Vector3.up, hitNormal) >= SLOPELIMIT) {
                print("here");
               
                float slideFriction = 0f;
                velocity.y = 0;
                velocity.x = ((1f - hitNormal.y) * hitNormal.x * (1.3f - slideFriction)) * gravity * Time.deltaTime * 25;
                velocity.z = ((1f - hitNormal.y) * hitNormal.x * (1.3f - slideFriction)) * gravity * Time.deltaTime * 25;
               /* player.ChangeValueMoveVectorX(((1f - hitNormal.y) * hitNormal.x * (1.3f - slideFriction)) * gravity * Time.deltaTime * 5);
                player.ChangeValueMoveVectorZ(((1f - hitNormal.y) * hitNormal.z * (1.3f - slideFriction)) * gravity * Time.deltaTime * 5);*/
            }
        } /*else if (hitNormal == Vector3.zero ) {
            //&& player.Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Fall A Loop"
            MoveCharacterWhenStuck(10);
        }*/
        hitNormal = Vector3.zero;
    }

    private bool IsGoingUp() {
        return velocity.y > 0;
    }

    void ApplyGravity() {
        if (!isGrounded) {
            velocity.y -= gravity * Time.deltaTime;
        } else {
            velocity.x = 0;
            velocity.z = 0;
            if(velocity.y < 0) {
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

    private bool IsInContactWithSlope() {
        RaycastHit hit;
       
        bool touchGround =  Physics.SphereCast(transform.position + new Vector3(0, 2.5f, 0), 1.75f, Vector3.down,out hit,1f);
        
        if(touchGround){
            hitNormal = hit.normal;
            Debug.DrawLine(hit.point, (hit.point + hit.normal*6),Color.cyan);
            print(hitNormal);
            if (Vector3.Angle(Vector3.up, hitNormal) <= SLOPELIMIT) {
                isGrounded = true;
                print("Slope allow to be grounded");
            }
        }
       
        return touchGround;
    }
    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
       
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + new Vector3(0, 1.5f, 0), 1.75f);
    }



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
        float distance = 2.25f;

        Vector3 dir = new Vector3(0, -distance, 0);
        Vector3 startPosition = transform.position;
        startPosition.y += 2f;
        Debug.DrawRay(startPosition, dir, Color.red, 1);

        bool isGroundeReturnValue;
        if (Physics.Raycast(startPosition, dir, out hit, distance))
        {
            isGroundeReturnValue = true;
            print("Grounded by raycast");
        }
        else
        {
            isGroundeReturnValue = false;
        }
       
        return isGroundeReturnValue;
    }
    

    public void PlaySound(AudioClip clipAudio)
    {
        /*if (player.IsGrounded)
        {
            sounds.PlaySound(clipAudio);
        }*/
    }/*
    public void Punch(AudioClip clipAudio)
    {

        if (HitCheck())
        {
            sounds.PlaySound(clipAudio);
        }

    }*/
   /* void OnAnimatorIK()
    {

        Vector3 rightShoulderLocation = new Vector3(0.5f, 7.5f, 0) + player.transform.position;
        Vector3 lookObj = player.TargetingVector;
        if (player.Anim)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    player.Anim.SetLookAtWeight(1);
                    player.Anim.SetLookAtPosition(player.LookAt);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightShoulderLocation != null)
                {

                    player.Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    player.Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    Vector3 gunPosition = player.TargetingVector.normalized * 3 + rightShoulderLocation;
                    player.Anim.SetIKPosition(AvatarIKGoal.RightHand, gunPosition);

                    Quaternion gunRotation = Quaternion.LookRotation(player.TargetingVector, Vector3.up);

                    player.Anim.SetIKRotation(AvatarIKGoal.RightHand, gunRotation);
                }

            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                player.Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                player.Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                player.Anim.SetLookAtWeight(0);
            }
        }
    }*/

}