using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : Personnage
{
    
   
    public Animator anim;
    public bool allowPlayerRotation= true;
    bool isCrouched;
    bool canMove;
    int walkForce;
    Vector3 moveVector;
    Vector3 fwd;
    
    private float delayBeforeNextFire = 0;
    private bool hasAGun = true;
  
    private Camera cam;
    private Vector3 targetingVector = new Vector3(0, 0, 1);
    private Vector3 lookAt = new Vector3(0, 8, 5);
    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }
    
    public Vector3 TargetingVector
    {
        get { return targetingVector; }
        set { targetingVector = value; }
    }
    public Vector3 LookAt
    {
        get { return lookAt; }
        set { lookAt = value; }
    }
    public Camera Cam
    {
        get { return cam; }
        set { cam = value; }
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
        cam = Camera.main;

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

                ShootGun();
            }
            else
            {
                fwd = raycastObject.transform.TransformDirection(Vector3.forward);
                anim.SetTrigger("isPunching");
            }

            delayBeforeNextFire = fireDelay;
        }
    }
    void ShootGun()
    {  
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit objectHit;
        if (Physics.Raycast(ray, out objectHit))
        {
            Vector3 bulletImpactLocation = objectHit.point;
            targetingVector = (bulletImpactLocation - (gameObject.transform.position + new Vector3(0.5f, 7.5f, 0)));
            Debug.DrawRay(gameObject.transform.position + new Vector3(0.5f, 7.5f, 0), targetingVector, Color.blue, 10);
            lookAt = bulletImpactLocation;
            if (objectHit.transform.tag == "Ennemi")
            {
                print("Toucher");
            }
            else 
            {
                print("Manquer");
            }
        }
    }
    public void Die()
    {

    }
    public void ApplyMoveInput()
    {
        if (isGrounded) {
            velocity.x = 0;
            velocity.z = 0;
            if (canMove == true) {
                if (Input.GetAxis("Vertical") != 0 && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Punching" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Mma Idle (1)" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Shooting" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Standing To Crouched") {
                    velocity += transform.forward * walkForce * Time.deltaTime * Mathf.Abs(Input.GetAxis("Vertical"));
                } else if (Input.GetAxis("Horizontal") != 0 && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Punching" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Mma Idle (1)" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Shooting" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Standing To Crouched") {
                    velocity += transform.forward * walkForce * Time.deltaTime * Mathf.Abs(Input.GetAxis("Horizontal"));
                }
            }
            if (Input.GetButtonDown("Jump") && IsCrouched == false) {
                velocity.y = Jump();
            }
            if (Input.GetKeyDown(KeyCode.C)) {
                Crouch();
            }

            if (Input.GetAxis("Fire1") != 0 && IsCrouched == false) {
                Attack();
            }
        }     
    }

    private void ApplyAnimation() {
        if (isGrounded) {
            Anim.SetBool("isFalling", false);
        } else {
            Anim.SetBool("isFalling", true);
        }
    }

    public void ResetMoveVector() {
        moveVector.x = 0;
        moveVector.z = 0;
    }

  
    public void TakeDammage()
    {

    }
    public float Jump()
    {
        float jumpForce =25;

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

    protected override void ApplyMovement() {
        ApplyMoveInput();
        SetAnimationCharacterSpeed();
        RotatePlayerAccordingToCamera();
    }

  

    void PlayerMoveAndRotation() {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        var camera = Camera.main;
        var foward = cam.transform.forward;
        var right = cam.transform.right;
        foward.y = 0f;
        right.y = 0f;
        foward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = foward * inputZ + right * inputX;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), DESIREDROTATIONSPEED);
    }
    void RotatePlayerAccordingToCamera() {

        if ((new Vector2(velocity.z, velocity.x)).magnitude>0 && allowPlayerRotation) {
            PlayerMoveAndRotation();
        }
    }

    void SetAnimationCharacterSpeed() {
        float speed = new Vector2(velocity.z, velocity.x).sqrMagnitude;  
        anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);   
    }

    protected override void OnUpdate() {
        ProcessFireDelay();
        ApplyAnimation();
    }

    private void ProcessFireDelay() {
        DelayBeforeNextFire -= Time.deltaTime;
    }
}
