using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControler : MonoBehaviour
{
    public MainPlayer player;
    public float inputX;
    public float inputZ;
    public float allowPlayerRotation;
    public float desiredRotationSpeed;
    private Camera cam;
    private float gravity = 65.0f;
    private Sounds sounds;
    private Vector3 hitNormal;
    private float verticalVel = 0;
    private bool ikActive = false;
    void Start()
    {

        player = GetComponent<MainPlayer>();
        print(player);
        cam = Camera.main;
        sounds = GetComponentInChildren<Sounds>();
    }

    void Update()
    {
        player.ChangeValueMoveVectorY(verticalVel);
        InputMagnitude();

        if (GroundCheck())
        {
            player.Anim.SetBool("isFalling", false);
            player.DelayBeforeNextFire -= Time.deltaTime;
            if (Input.GetButtonDown("Jump") && player.IsCrouched == false)
            {
                verticalVel = player.Jump();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                player.Crouch();
            }
            if (Input.GetAxis("Fire1") != 0)
            {
                ikActive = true;
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow,10);
               
                player.Attack();
            }

            player.Move();
        }
        else
        {
            player.Anim.SetBool("isFalling", true);
            verticalVel -= gravity * Time.deltaTime;
            Slide();
        }

        player.Controller.Move(player.MoveVector * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    void Slide()
    {

        if (hitNormal != Vector3.zero)
        {
            float slopeLimit = 50;
            if (verticalVel < 0 && Vector3.Angle(Vector3.up, hitNormal) >= slopeLimit)
            {
                float slideFriction = 0f;
                verticalVel = 0;
                player.ChangeValueMoveVectorX(((1f - hitNormal.y) * hitNormal.x * (1.3f - slideFriction)) * gravity * Time.deltaTime * 5);
                player.ChangeValueMoveVectorZ(((1f - hitNormal.y) * hitNormal.z * (1.3f - slideFriction)) * gravity * Time.deltaTime * 5);
            }
        }
        else if (hitNormal == Vector3.zero && player.Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Fall A Loop")
        {
            MoveCharacter(10);
        }
        hitNormal = Vector3.zero;
    }

    void MoveCharacter(float force)
    {

        player.ChangeValueMoveVectorX(0);
        player.ChangeValueMoveVectorZ(0);

        player.MoveVector += transform.forward * force * Time.deltaTime * Mathf.Abs(10);

        player.MoveVector += transform.forward * force * Time.deltaTime * Mathf.Abs(10);

        player.MoveVector += transform.forward * force * Time.deltaTime * Mathf.Abs(-20);

        player.MoveVector += transform.forward * force * Time.deltaTime * Mathf.Abs(-20);


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
        player.SetFloatZ(inputZ);
        player.SetFloatX(inputX);
        float speed = new Vector2(inputZ, inputX).sqrMagnitude;

        //Déplacer le joueur     
        player.Anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
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
        //Debug.DrawRay(startPosition, dir, Color.red, 1);

        if (Physics.Raycast(startPosition, dir, out hit, distance))
        {
            player.IsGrounded = true;
        }
        else
        {
            player.IsGrounded = false;
        }
        return player.IsGrounded;
    }
    bool HitCheck()
    {
        bool hitDetected = false;
        RaycastHit objectHit;
        Debug.DrawRay(player.RaycastObject.transform.position + new Vector3(0, 5, 0), player.Fwd * 3, Color.green, 2);
        //Physics.Raycast(raycastObject.transform.position, fwd, out objectHit, 7
        //Physics.SphereCast(transform.position + new Vector3(0, controller.height / 2, 0), controller.height / 2, transform.forward, out objectHit, 10)
        if (Physics.Raycast(player.RaycastObject.transform.position + new Vector3(0, 5, 0), player.Fwd, out objectHit, 3))
        {


            hitDetected = true;
        }
        else
        {
            hitDetected = false;

        }
        return hitDetected;
    }

    public void PlaySound(AudioClip clipAudio)
    {
        if (player.IsGrounded)
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
    void OnAnimatorIK()
    {
        
        Transform rightHandObj = null;
        Transform lookObj = null;
        if (player.Anim)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {
                
                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    player.Anim.SetLookAtWeight(1);
                    player.Anim.SetLookAtPosition(lookObj.position);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightHandObj != null)
                {

                    player.Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    player.Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    player.Anim.SetIKPosition(AvatarIKGoal.RightHand, player.transform.position);
                    player.Anim.SetIKRotation(AvatarIKGoal.RightHand, cam.transform.rotation);
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
    }

}