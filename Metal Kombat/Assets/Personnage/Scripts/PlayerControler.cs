using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControler : MonoBehaviour
{

    //    private CharacterController controller;
    //    public bool isGrounded;
    //    private Animator anim;
    //private bool isCrouched = false;
    // private bool canMove = true;
    //    private int walkForce = 1500;
    // private Vector3 moveVector;

    private MainPlayer player;


    public GameObject raycastObject;
    public float inputX;
    
    public float inputZ;
    public float allowPlayerRotation;
    public float desiredRotationSpeed;


    private Camera cam;
    
    private float gravity = 65.0f;
    private float jumpForce = 20.0f;
    
    private float crouchStartTime;
    private bool isCrouching = false;
    private Sounds sounds;
    private Vector3 hitNormal;
    private float fireDelay = 0.5f;
    private float delayBeforeNextFire = 0;
    private float verticalVel = 0;
    private Vector3 fwd;
   
   
    private float slopeLimit = 50;
    private float slideFriction = 0f;

    void Start()
    {
        //anim = this.GetComponent<Animator>();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();
        sounds = GetComponentInChildren<Sounds>();
    }

    void Update()
    {
        player.ChangeValueMoveVectorY(verticalVel);
        InputMagnitude();

        if (GroundCheck())
        {
            player.Anim.SetBool("isFalling", false);
            delayBeforeNextFire -= Time.deltaTime;
            if (Input.GetButtonDown("Jump") && player.IsCrouched == false)
            {
                player.Jump();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                player.Crouch();
            }
            if (Input.GetAxis("Fire1") != 0)
            {
                player.Attack(delayBeforeNextFire);
            }

            player.Move();
        }
        else
        {
            anim.SetBool("isFalling", true);
            verticalVel -= gravity * Time.deltaTime;
            Slide();
        }

        controller.Move(moveVector * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        hitNormal = hit.normal;
    }

    void Slide()
    {

        if (hitNormal != Vector3.zero)
        {
            if (verticalVel < 0 && Vector3.Angle(Vector3.up, hitNormal) >= slopeLimit)
            {

                verticalVel = 0;
                player.MoveVector.x = (((1f - hitNormal.y) * hitNormal.x * (1.3f - slideFriction)) * gravity * Time.deltaTime * 10);
                player.MoveVector.z = (((1f - hitNormal.y) * hitNormal.z * (1.3f - slideFriction)) * gravity * Time.deltaTime * 10);
            }
        }
        else if (hitNormal == Vector3.zero && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Fall A Loop")
        {
            MoveCharacter(10);
        }
        hitNormal = Vector3.zero;


    }
    
    void MoveCharacter(float force)
    {

        moveVector.x = 0;
        moveVector.z = 0;

        player.MoveVector += transform.forward * force * Time.deltaTime * Mathf.Abs(10);

        player.MoveVector += transform.forward * force * Time.deltaTime * Mathf.Abs(10);

        player.MoveVector += transform.forward * force * Time.deltaTime * Mathf.Abs(-20);

        player.MoveVector += transform.forward * force * Time.deltaTime * Mathf.Abs(-20);


    }
    void Jump()
    {
        verticalVel = jumpForce;
        player.Anim.SetTrigger("isJumping");
    }
    
    void Attack()
    {

        if (delayBeforeNextFire <= 0)
        {
            fwd = raycastObject.transform.TransformDirection(Vector3.forward);
            player.Anim.SetTrigger("isPunching");
            delayBeforeNextFire = fireDelay;
        }

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

        Vector3 desiredMoveDirection = foward * inputZ + right * inputX;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }

    void InputMagnitude()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        anim.SetFloat("InputZ", inputZ, 0.0f, Time.deltaTime * 2f);
        anim.SetFloat("InputX", inputX, 0.0f, Time.deltaTime * 2f);
        float speed = new Vector2(inputZ, inputX).sqrMagnitude;

        //Déplacer le joueur     
        anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
        if (speed > allowPlayerRotation)
        {
            PlayerMoveAndRotation();
        }
    }

    bool GroundCheck()
    {
        RaycastHit hit;
        float distance = 2.75f;

        Vector3 dir = new Vector3(0, -distance, 0);
        Vector3 startPosition = transform.position;
        startPosition.y += 2f;
        Debug.DrawRay(startPosition, dir, Color.red, 1);

        if (Physics.Raycast(startPosition, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        return isGrounded;
    }
    bool HitCheck()
    {
        bool hitDetected = false;
        RaycastHit objectHit;
        Debug.DrawRay(raycastObject.transform.position + new Vector3(0, 5, 0), fwd * 3, Color.green, 2);
        //Physics.Raycast(raycastObject.transform.position, fwd, out objectHit, 7
        //Physics.SphereCast(transform.position + new Vector3(0, controller.height / 2, 0), controller.height / 2, transform.forward, out objectHit, 10)
        if (Physics.Raycast(raycastObject.transform.position + new Vector3(0, 5, 0), fwd, out objectHit, 3))
        {


            hitDetected = true;
        }
        else
        {
            hitDetected = false;

        }
        return hitDetected;
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
            yield return new WaitForSeconds(0.1f);
        }
        isCrouching = false;
    }

    public void PlaySound(AudioClip clipAudio)
    {
        if (isGrounded)
        {
            sounds.PlaySound(clipAudio);
        }
    }
    public void Punch(AudioClip clipAudio)
    {

        if (HitCheck())
        {
            sounds.PlaySound(clipAudio);
        }

    }

}