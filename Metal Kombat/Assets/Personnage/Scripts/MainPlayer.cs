using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : Personnage
{
    CharacterController controller;
    bool isGrounded;
    Animator anim;
    bool isCrouched;
    bool canMove;
    int walkForce;
    Vector3 moveVector;

    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }
    public CharacterController Controller
    {
        get { return controller; }
        set { controller = value; }
    }
    public Animator Anim
    {
        get { return anim; }
        set { anim = value; }
    }
    public bool IsCrouched
    {
        get { return isCrouched; }
        set { isCrouched = value; }
    }
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    public int WalkForce
    {
        get { return walkForce; }
        set { walkForce = value; }
    }
    public Vector3 MoveVector
    {
        get { return moveVector; }
        set { moveVector = value; }
    }



    public MainPlayer()
    {
        anim = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();
        isCrouched = false;
        canMove = true;
        walkForce = 1500;
        isGrounded = false;
        moveVector = new Vector3();
    }

    public void Attack(int delayBeforeNextFire)
    {
        if (delayBeforeNextFire <= 0)
        {
            fwd = raycastObject.transform.TransformDirection(Vector3.forward);
            anim.SetTrigger("isPunching");
            delayBeforeNextFire = fireDelay;
        }
    }
    public void Die()
    {

    }
    public void Move()
    {
        if (canMove == true)
        {
            moveVector.x = 0;
            moveVector.z = 0;

            if (Input.GetAxis("Vertical") != 0 && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Punching")
            {
                moveVector += transform.forward * walkForce * Time.deltaTime * Mathf.Abs(Input.GetAxis("Vertical"));
            }
            else if (Input.GetAxis("Horizontal") != 0 && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Punching")
            {
                moveVector += transform.forward * walkForce * Time.deltaTime * Mathf.Abs(Input.GetAxis("Horizontal"));
            }
        }
    }
    public void TakeDammage()
    {

    }
    public void Jump()
    {
    }
    public void Crouch()
    {
        if (canMove == true)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
        if (isCrouched)
        {
            isCrouched = false;
        }
        else
        {
            isCrouched = true;
        }
        anim.SetBool("isCrouched", isCrouched);
        float crouchStartTime = Time.time;
    }
    public float MoveVectorY()
    {
        return moveVector.y;
    }
    public float MoveVectorX()
    {
        return moveVector.x;
    }
    public float MoveVectorZ()
    {
        return moveVector.z;
    }
    public void ChangeValueMoveVectorY(float y)
    {
        moveVector.y = y;
    }
    public void ChangeValueMoveVectorX(float x)
    {
        moveVector.x = x;
    }
    public void ChangeValueMoveVectorZ(float z)
    {
        moveVector.z = z;
    }
}
