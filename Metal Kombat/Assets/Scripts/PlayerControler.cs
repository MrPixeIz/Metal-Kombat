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

    void Start()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();
       
    }

    void Update()
    {
        inputMagnitude();

        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetTrigger("isJumping");     
            }
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("isPunching");
            }
        }
        else
        {
            verticalVel -= 2;

        }
        moveVector = new Vector3(0, verticalVel, 0);
        controller.Move(moveVector);
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
}