using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiDetectionScript : MonoBehaviour
{

    public Animator anim;
    public Transform playerTarget;
    public GameObject startPatrolObject;
    public GameObject endPatrolObject;

    private float DistancePlayer;
    private float DistanceEndPatrol;
    private float DistanceStartPatrol;
    private float chaseRange = 30;
    private float attackRange = 5;
    private float speed;
    private Transform startPatrol;
    private Transform endPatrol;
    private GameObject patrolTarget;
    private float switchToRange = 4;
    private RaycastHit hit;
    private Sounds sounds;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        patrolTarget = GameObject.Find("EndPatrol");
    }

    void Update()
    {
        startPatrol = GameObject.Find("StartPatrol").transform;
        endPatrol = GameObject.Find("EndPatrol").transform;
        playerTarget = GameObject.Find("Player").transform;
        DistancePlayer = Vector3.Distance(playerTarget.position, transform.position);
        DistanceStartPatrol = Vector3.Distance(startPatrol.position, transform.position);
        DistanceEndPatrol = Vector3.Distance(endPatrol.position, transform.position);

        VerifyIfHitSomething();

        if (DistancePlayer <= chaseRange && DistancePlayer >= attackRange)
        {
            ResetBool();
            Chase();
        }

        if (DistancePlayer > chaseRange)
        {
            ResetBool();
            //Idle();
            Patrol(patrolTarget);
        }

        if (DistancePlayer <= attackRange)
        {
            Attack();
        }
    }

    #region Movement
    public void Idle()
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
        Vector3 lookat = new Vector3(pGameObject.transform.position.x, gameObject.transform.position.y, pGameObject.transform.position.z);
        transform.LookAt(lookat);
    }

    void Chase()
    {
        MoveTo(playerTarget);
    }

    public void Patrol(GameObject gameObject)
    {
        print(gameObject.name);
        // print(DistanceEndPatrol + ", " + DistanceStartPatrol);
        MoveTo(gameObject.transform);
    }

    public void SetPatrol(GameObject newPatrolTarget)
    {
        patrolTarget = newPatrolTarget;
    }

    #endregion

    #region SetBool

    void ResetBool()
    {
        anim.SetBool("isPunching", false);
        anim.SetBool("isIdlePunching", false);
    }

    #endregion

    #region Events
    void VerifyIfHitSomething()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            print(ReachPatrol.FindObjectOfType<GameObject>().gameObject);
            if (patrolTarget == endPatrolObject)
            {
                SetPatrol(startPatrolObject);
            }
            else
            {
                SetPatrol(endPatrolObject);
            }

        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        sounds.PlaySound(audioClip);
    }
    #endregion
    

}