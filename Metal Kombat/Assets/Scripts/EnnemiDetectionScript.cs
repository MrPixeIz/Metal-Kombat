using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiDetectionScript : MonoBehaviour
{
    public float inputX;
    public float inputZ;
    private float DistancePlayer;
    private float DistanceEndPatrol;
    private float DistanceStartPatrol;
    public Transform playerTarget;
    public float chaseRange = 30;
    public float attackRange = 5;
    public Animator anim;
    public float speed;
    public Vector3 desiredMoveDirection;
    public float desiredRotationSpeed;
    public Transform startPatrol;
    public Transform endPatrol;
    public float switchToRange = 4;

    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        startPatrol = GameObject.Find("StartPatrol").transform;
        endPatrol = GameObject.Find("EndPatrol").transform;
        playerTarget = GameObject.Find("Player").transform;
        DistancePlayer = Vector3.Distance(playerTarget.position, transform.position);
        DistanceStartPatrol = Vector3.Distance(startPatrol.position, transform.position);
        DistanceEndPatrol = Vector3.Distance(endPatrol.position, transform.position);
       
        if (DistancePlayer > chaseRange)
        {
            ResetBool();
            //Idle();
            Patrol();
        }

        if (DistancePlayer <= chaseRange && DistancePlayer >= attackRange)
        {
            ResetBool();
            Chase();
        }

        if (DistancePlayer <= attackRange)
        {
            Attack();
        }
    }

    #region Movement
    void Idle()
    {
        anim.SetFloat("InputMagnitude", 0, 0.0f, Time.deltaTime);
    }

    void Attack()
    {
        anim.SetBool("isPunching", true);
        IdlePunching();
    }

    void IdlePunching()
    {
        anim.SetBool("isIdlePunching", true);
    }

    #endregion

    #region Move
    void MoveTo(Transform pGameObject)
    {
        CharacterController controller = this.GetComponent<CharacterController>();

        speed = 3f;

        anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
        var forward = transform.TransformDirection(Vector3.forward);
        controller.SimpleMove(forward * speed);
        transform.LookAt(pGameObject);
    }

    void Chase()
    {
        MoveTo(playerTarget);
    }

    void Patrol()
    {
        //Mettre un bool qui dit s'il est rendu vire de bord parce que sinon il est entre deux phases.
        if (DistanceEndPatrol <= switchToRange)
        {
            MoveTo(startPatrol);
        }

        if(DistanceEndPatrol >= switchToRange)
        {
            MoveTo(endPatrol);
        }
    }

    #endregion

    #region SetBool

    void ResetBool()
    {
        anim.SetBool("isPunching", false);
        anim.SetBool("isIdlePunching", false);
    }

    #endregion

}