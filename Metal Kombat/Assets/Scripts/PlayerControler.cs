using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
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
    private float jumpForce = 15.0f;
    private bool isCrouched = false;
    private float crouchStartTime;
    private bool isCrouching = false;
    private Sounds sounds;
    


    void Start()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();
        sounds = GetComponentInChildren<Sounds>();
    }

    void Update()
    {
        inputMagnitude();
        isGrounded = GroundCheck();
        if (isGrounded)
        {
            anim.SetBool("isFalling", false);
            verticalVel = -gravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump") && isCrouched == false)
            {
                verticalVel = jumpForce;
                anim.SetTrigger("isJumping");
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (!isCrouching)
                {
                    isCrouching = true;
                    if (isCrouched)
                    {
                        isCrouched = false;
                    }
                    else
                    {
                        isCrouched = true;
                    }
                    anim.SetBool("isCrouched", isCrouched);
                    crouchStartTime = Time.time;
                    if (isCrouched)
                    {
                        print("going down");
                        StartCoroutine(UpdateHeight(17, 10, 0.5f));
                    }
                    else
                    {
                        print("going up");
                        StartCoroutine(UpdateHeight(10, 17, 0.1f));
                    }
                }
               
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
            verticalVel -= gravity * Time.deltaTime;
            anim.SetBool("isFalling", true);
        }
      
        moveVector = new Vector3(0, verticalVel, 0);
        controller.SimpleMove(moveVector * Time.deltaTime);

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
        float distance2 = 2.5f;
        Vector3 dir = new Vector3(0, -1);
        Vector3 startPosition = transform.position;
        startPosition.y += 2f;
        Debug.DrawRay(startPosition, dir, Color.red);



        if (Physics.Raycast(startPosition, dir, out hit, distance1) || Physics.Raycast(startPosition, dir, out hit, distance2))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        return isGrounded;
    }
    IEnumerator UpdateHeight(float startHeight, float endHeight, float time)
    {
        float delta = Time.time - crouchStartTime;
        while (delta < 1)
        {
            delta = Time.time - crouchStartTime;
            float percentCompletion = delta / time;
            controller.height = Mathf.Lerp(startHeight, endHeight, percentCompletion);
           
            Mathf.Lerp(17, 10, delta);

            print(Mathf.Lerp(17, 10, delta));
            yield return new WaitForSeconds(0.1f);
           
        }
        isCrouching = false;
    }

    public void PlaySound(AudioClip clipAudio)
    {
        //sounds.PlaySound(clipAudio);
    }

}