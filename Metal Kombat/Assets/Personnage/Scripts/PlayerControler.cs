using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControler : MonoBehaviour
{

    public float inputX;
    public float inputZ;
    public float desiredRotationSpeed;
    private Animator anim;
    public float allowPlayerRotation;
    private Camera cam;
    private CharacterController controller;
    public bool isGrounded;
    private float gravity = 65.0f;
    private float jumpForce = 50.0f;
    private bool isCrouched = false;
    private float crouchStartTime;
    private bool isCrouching = false;
    private Sounds sounds;
    private Vector3 hitNormal;
    private float fireDelay = 0.5f;
    private float delayBeforeNextFire = 0;
    public GameObject raycastObject;
    private float verticalVel = 0;
    Vector3 fwd;
    private bool canMove = true;
    private int walkForce = 500;
    private Vector3 moveVector;
    private float slopeLimit = 80;
    private float slideFriction = 0f;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();
        sounds = GetComponentInChildren<Sounds>();
    }

    void Update()
    {
        moveVector.y = verticalVel;

        InputMagnitude();
        isGrounded = GroundCheck();

        if (isGrounded)
        {
            anim.SetBool("isFalling", false);
            delayBeforeNextFire -= Time.deltaTime;
            if (Input.GetButtonDown("Jump") && isCrouched == false)
            {

                Jump();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {

                Crouch();
            }
            if (Input.GetAxis("Fire1") != 0)
            {
                Attack();
            }
            MoveCharacter();
        }
        else
        {
            anim.SetBool("isFalling", true);
          
            Slide();
        }

        verticalVel -= gravity * Time.deltaTime;
        controller.Move(moveVector * Time.deltaTime);
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }
    void Slide()
    {
        if (Vector3.Angle(Vector3.up, hitNormal) <= slopeLimit)
        {
            moveVector.x = (((1f - hitNormal.y) * hitNormal.x * (1f - slideFriction)) * 10);
            moveVector.z = (((1f - hitNormal.y) * hitNormal.z * (1f - slideFriction)) * 10);
        }
    }
    void MoveCharacter()
    {
        if (canMove == true)
        {
            moveVector.x = 0;
            moveVector.z = 0;
            if (Input.GetAxis("Vertical") != 0)
            {
                moveVector += transform.forward * walkForce * Time.deltaTime * Mathf.Abs(Input.GetAxis("Vertical"));
            }
            if (Input.GetAxis("Horizontal") != 0)
            {
                moveVector += transform.forward * walkForce * Time.deltaTime * Mathf.Abs(Input.GetAxis("Horizontal"));
            }
        }
    }
    void Jump()
    {
        verticalVel = jumpForce;
        anim.SetTrigger("isJumping");
    }
    void Crouch()
    {
        if (canMove == true)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
        //if (!isCrouching)
        //{
        print("Crouch Function");
        //isCrouching = true;
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
        if (isCrouched)
        {
            print("crouch");
            //StartCoroutine(UpdateHeight(17, 10, 0.5f));
        }
        else
        {
            // StartCoroutine(UpdateHeight(10, 17, 0.1f));
            print("uncrouch");
        }
        //}
    }
    void Attack()
    {

        if (delayBeforeNextFire <= 0)
        {
            fwd = raycastObject.transform.TransformDirection(Vector3.forward);
            anim.SetTrigger("isPunching");
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
        float distance1 = 0.1f;
        float distance2 = 4f;
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
            print(Mathf.Lerp(startHeight, endHeight, percentCompletion));
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