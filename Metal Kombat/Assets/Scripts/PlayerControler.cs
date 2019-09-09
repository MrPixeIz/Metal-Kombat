using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    public float inputX;
    public float inputZ;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed;
    public Animator anim;
    public float speed;
    public float allowPlayerRotation;
    public Camera cam;
    public CharacterController controller;
    public bool isGrounded;
    private float verticalVel;
    private Vector3 moveVector;
    private float gravity = 20.0f;
    private float jumpForce = 10.0f;
    private bool isCrouched = false;


    void Start()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        inputMagnitude();

        isGrounded = GroundCheck();
        if (isGrounded)
        {
            anim.SetBool("isFalling", false);
            verticalVel = -gravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump") && isCrouched==false)
            {
                verticalVel = jumpForce;
                anim.SetTrigger("isJumping");
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (isCrouched)
                {
                    isCrouched = false;
                }
                else
                {
                    isCrouched = true;
                }
                anim.SetBool("isCrouched", isCrouched);
            }
            

            //****************************************************************************************************************************
            //****************************************************************************************************************************
            //Retirer GetMouseButton pour axis
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("isPunching");
            }
            //****************************************************************************************************************************
            //****************************************************************************************************************************

        }
        else
        {
            verticalVel -=gravity*Time.deltaTime ;
            //anim.SetBool("isFalling",true);
        }
        moveVector = new Vector3(0, verticalVel, 0);
        controller.Move(moveVector * Time.deltaTime);
    }

    void PlayerMoveAndRotation()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        var camera = Camera.main;
        var foward = cam.transform.forward;
        var right = cam.transform.right;

        foward.y = 0f;
        right.y = 0f;

        foward.Normalize();
        right.Normalize();

        desiredMoveDirection = foward * inputZ + right * inputX;
        if (blockRotationPlayer == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
        }
    }

    void inputMagnitude()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        anim.SetFloat("InputZ", inputZ, 0.0f, Time.deltaTime * 2f);
        anim.SetFloat("InputX", inputX, 0.0f, Time.deltaTime * 2f);

        speed = new Vector2(inputZ, inputX).sqrMagnitude;

        //Déplacer le joueur

        if (speed > allowPlayerRotation)
        {
            anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
            PlayerMoveAndRotation();

        }
        else if (speed < allowPlayerRotation)
        {
            anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
        }
    }

    bool GroundCheck()
    {
        RaycastHit hit;
        float distance1 = 0.1f;
        float distance2 = 1.95f;
        Vector3 dir = new Vector3(0, -1);

        Debug.DrawRay(transform.position, dir, Color.red);

        if (Physics.Raycast(transform.position, dir, out hit, distance1) || Physics.Raycast(transform.position, dir, out hit, distance2))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        return isGrounded;
    }


}