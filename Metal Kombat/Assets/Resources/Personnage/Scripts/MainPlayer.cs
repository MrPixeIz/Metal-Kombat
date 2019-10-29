using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainPlayer : Personnage, iDamageable
{
    
    private bool ikActive = false;
    Vector3 moveVector;
    private float delayBeforeNextFire = 0;
    private bool hasAGun = true;
    private OnDieMainPlayerHook onDieMainPlayerHook;
    private Camera cam;
    private Vector3 targetingVector = new Vector3(0, 0, 1);
    private Vector3 lookAt = new Vector3(0, 8, 5);
    private float timeForIkActive = 0;
    private RaycastHit objectHit;
    private AudioClip ShootSoundclip;

    public int DamageAmount
    {
        get
        {
            return 50;
        }

        set
        {
            DamageAmount = value;
        }
    }

    //bool canMove = true;
    //Vector3 fwd;
    public MainPlayer()
    {

        isCrouched = false;
        //canMove = true;
        
        isGrounded = false;
        moveVector = new Vector3();

    }


    protected override void OnStart()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
        sounds = GetComponentInChildren<Sounds>();
        ShootSoundclip = Resources.Load<AudioClip>("Personnage/Sons/gun");
    }

    protected override void SetupLifeBar()
    {
        const float PLAYERMAXSTARTINGLIFE = 100;
        Image barreVie = GameObject.FindGameObjectWithTag("viePleine").GetComponent<Image>();
        barreDeVie = new LifeBar(barreVie, PLAYERMAXSTARTINGLIFE);
        onDieMainPlayerHook = new OnDieMainPlayerHook(this);

    }

    protected override void Attack()
    {

        if (delayBeforeNextFire <= 0)
        {

            float fireDelay = 0.5f;
            if (hasAGun)
            {
                Camera cam = Camera.main;

                //anim.SetTrigger("isShooting");
                ikActive = true;
                timeForIkActive = 1;
                ShootGun();

                PlaySound(ShootSoundclip);
            }
            else
            {
                //fwd = raycastObject.transform.TransformDirection(Vector3.forward);
                anim.SetTrigger("isPunching");

            }

            delayBeforeNextFire = fireDelay;
        }
    }
    void ShootGun()
    {

        UpdateViserHitLocation();
        iDamageable ennemi = objectHit.collider.gameObject.GetComponent<iDamageable>();
        if(ennemi != null)
        {
            ennemi.TakeDammageInt(DamageAmount);
        }
    
        //if (objectHit.transform.tag == "Ennemi")
        //{
            
        //}
        //else
        //{
        //    print("Manquer");
        //}

    }
    private void UpdateViserHitLocation()
    {

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        if (Physics.Raycast(ray, out objectHit))
        {
            Vector3 bulletImpactLocation = objectHit.point;
            targetingVector = (bulletImpactLocation - (gameObject.transform.position + new Vector3(0.5f, 7.5f, 0)));
            Debug.DrawRay(gameObject.transform.position + new Vector3(0.5f, 7.5f, 0), targetingVector, Color.blue, 10);
            lookAt = bulletImpactLocation;

        }
        Vector3 newLookAt = lookAt;
        newLookAt.y = 0;
        transform.LookAt(newLookAt);

    }
    protected override void Die()
    {
        print("MAIN PLAYER DIE");
        SceneManager.LoadScene("mapLevel1");
        //anim.SetTrigger("isDying");
    }
    protected override void ApplyMoveInput()
    {
        if (isGrounded)
        {
            velocity.x = 0;
            velocity.z = 0;
            int walkForce=1500;
            /*if (canMove == true)
            {*/
            if (Input.GetAxis("Vertical") != 0 && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Punching" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Mma Idle (1)" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Shooting" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Standing To Crouched")
            {
                velocity += transform.forward * walkForce * Time.deltaTime * Mathf.Abs(Input.GetAxis("Vertical"));
            }
            else if (Input.GetAxis("Horizontal") != 0 && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Punching" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Mma Idle (1)" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Shooting" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Standing To Crouched")
            {
                velocity += transform.forward * walkForce * Time.deltaTime * Mathf.Abs(Input.GetAxis("Horizontal"));
            }
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Jump();
        }
        /*if (Input.GetKeyDown(KeyCode.C)) {
            Crouch();
        }*/
        if (Input.GetKeyDown(KeyCode.V))
        {
            //damageable.TakeDammageInt();
        }

        if (Input.GetAxis("Fire1") != 0 && isCrouched == false)
        {
            Attack();
        }
        /*}*/
    }

    private void ApplyAnimation()
    {
        if (isGrounded)
        {
            anim.SetBool("isFalling", false);
        }
        else
        {
            anim.SetBool("isFalling", true);
        }
    }

    public void ResetMoveVector()
    {
        moveVector.x = 0;
        moveVector.z = 0;
    }


    protected override void TakeDammage()
    {
        float damage = 10;
        barreDeVie.ModifyHealthWithValue(-damage);
        /*float damage = 10;
        if (pointsDeVie - damage >= 0)
        {
            pointsDeVie -= damage;
           
        }
        else
        {
            //Die();
        }*/
    }
    public float Jump()
    {
        float jumpForce = 25;

        anim.SetTrigger("isJumping");
        return jumpForce;
    }

    /*public void Crouch()
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
    }*/

    protected override void ApplyMovement()
    {
        ApplyMoveInput();
        SetAnimationCharacterSpeed();
        RotatePlayerAccordingToCamera();
    }



    void PlayerMoveAndRotation()
    {
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
    void RotatePlayerAccordingToCamera()
    {

        if ((new Vector2(velocity.z, velocity.x)).magnitude > 0)
        {
            PlayerMoveAndRotation();
        }
    }

    void SetAnimationCharacterSpeed()
    {
        float speed = new Vector2(velocity.z, velocity.x).sqrMagnitude;
        anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
    }

    protected override void OnUpdate()
    {
        ProcessFireDelay();
        ApplyAnimation();
        PlaceGun();
    }
    private void PlaceGun()
    {
        if (ikActive)
        {
            UpdateViserHitLocation();

            timeForIkActive -= Time.deltaTime;
            if (timeForIkActive <= 0)
            {
                ikActive = false;
            }
        }
    }
    private void ProcessFireDelay()
    {
        delayBeforeNextFire -= Time.deltaTime;
    }
    void OnAnimatorIK()
    {

        Vector3 rightShoulderLocation = new Vector3(0.5f, 7.5f, 0) + transform.position;
        Vector3 lookObj = targetingVector;
        if (anim)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    anim.SetLookAtWeight(1);
                    anim.SetLookAtPosition(lookAt);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightShoulderLocation != null)
                {

                    anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    Vector3 gunPosition = targetingVector.normalized * 3 + rightShoulderLocation;
                    anim.SetIKPosition(AvatarIKGoal.RightHand, gunPosition);

                    Quaternion gunRotation = Quaternion.LookRotation(targetingVector, Vector3.up);

                    anim.SetIKRotation(AvatarIKGoal.RightHand, gunRotation);
                }

            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                anim.SetLookAtWeight(0);
            }
        }
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

        if (MeleeHitCheck())
        {
            sounds.PlaySound(clipAudio);
        }

    }

    protected override OnDieHook GetOnDieEvent()
    {
        return onDieMainPlayerHook;
    }

    void iDamageable.TakeDammageInt(int dammage)
    {
        TakeDammage();
    }

    protected class OnDieMainPlayerHook : OnDieHook
    {
        MainPlayer mainPlayer;
        public OnDieMainPlayerHook(MainPlayer inMainPlayer)
        {
            mainPlayer = inMainPlayer;
        }

        public override void OnDieEvent()
        {
            mainPlayer.Die();
        }
    }
}
