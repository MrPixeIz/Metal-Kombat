using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : Personnage
{
    public CharacterController controller;
    bool isGrounded;
    public Animator anim;
    bool isCrouched;
    bool canMove;
    int walkForce;
    Vector3 moveVector;
    Vector3 fwd;
    public GameObject raycastObject;
    private float delayBeforeNextFire = 0;
    private bool hasAGun = true;
    //onAminationIK
    //camera.screenpointtoray
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
    public Vector3 Fwd
    {
        get { return fwd; }
        set { fwd = value; }
    }
    public GameObject RaycastObject
    {
        get { return raycastObject; }
        set { raycastObject = value; }
    }
    public float DelayBeforeNextFire
    {
        get { return delayBeforeNextFire; }
        set { delayBeforeNextFire = value; }
    }
    public bool HasAGun
    {
        get { return hasAGun; }
        set { hasAGun = value; }
    }

    public MainPlayer()
    {

        isCrouched = false;
        canMove = true;
        walkForce = 1500;
        isGrounded = false;
        moveVector = new Vector3();

    }
    void Start()
    {
        anim = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();

    }

    public void Attack()
    {

        if (delayBeforeNextFire <= 0)
        {

            float fireDelay = 0.5f;
            if (HasAGun)
            {
                Camera cam = Camera.main;

                anim.SetTrigger("isShooting");
                fwd = raycastObject.transform.TransformDirection(cam.transform.position);
                Debug.DrawRay(raycastObject.transform.position + new Vector3(0, 5, 0), fwd, Color.green, 5);
            }
            else
            {
                fwd = raycastObject.transform.TransformDirection(Vector3.forward);
                anim.SetTrigger("isPunching");
            }

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

            if (Input.GetAxis("Vertical") != 0 && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Punching" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Mma Idle (1)")
            {
                moveVector += transform.forward * walkForce * Time.deltaTime * Mathf.Abs(Input.GetAxis("Vertical"));
            }
            else if (Input.GetAxis("Horizontal") != 0 && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Punching" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Mma Idle (1)")
            {
                moveVector += transform.forward * walkForce * Time.deltaTime * Mathf.Abs(Input.GetAxis("Horizontal"));
            }
        }
    }
    public void TakeDammage()
    {

    }
    public float Jump()
    {
        float jumpForce = 20;

        anim.SetTrigger("isJumping");
        return jumpForce;
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
    public void SetFloatZ(float inputZ)
    {
        anim.SetFloat("InputZ", inputZ, 0.0f, Time.deltaTime * 2f);
    }
    public void SetFloatX(float inputX)
    {
        anim.SetFloat("InputZ", inputX, 0.0f, Time.deltaTime * 2f);
    }
}
