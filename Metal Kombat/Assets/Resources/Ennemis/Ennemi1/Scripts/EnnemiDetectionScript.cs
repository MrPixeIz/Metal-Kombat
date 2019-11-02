using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnnemiDetectionScript : MonoBehaviour, iDamageable
{

    public Animator anim;
    public GameObject playerTarget;
    public GameObject startPatrolObject;
    public GameObject endPatrolObject;

    private float DistancePlayer;
    private float chaseRange = 30;
    private float attackRange = 5;
    private float soundRange = 60;
    private GameObject patrolTarget;
    private Sounds sounds;
    private float pointsDeVie = 100;
    iDamageable damageable;
    EnemiesHitPointManager enemiesHitPointManager;
    public int DamageAmount
    {
        get
        {
            return 5;
        }

        set
        {
            DamageAmount = value;
        }
    }

    void Start()
    {
        anim = this.GetComponent<Animator>();
        patrolTarget = GameObject.Find("EndPatrol");
        sounds = GetComponentInChildren<Sounds>();
        enemiesHitPointManager = GetComponentInChildren<EnemiesHitPointManager>();

    }

    void Update()
    {
        playerTarget = GameObject.Find("Player");
        DistancePlayer = Vector3.Distance(playerTarget.transform.position, transform.position);


        if (pointsDeVie == 100)
        {
            VerifyIfHitSomething();
            if (DistancePlayer <= chaseRange && DistancePlayer >= attackRange)
            {
                ResetBool();
                Chase();
            }

            if (DistancePlayer > chaseRange)
            {
                ResetBool();
                Patrol(patrolTarget);
            }
        }
        else
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
    public void Idle()
    {
        anim.SetFloat("InputMagnitude", 0, 0.0f, Time.deltaTime);
    }

    void Attack()
    {
        Vector3 lookat = new Vector3(playerTarget.transform.position.x, gameObject.transform.position.y, playerTarget.transform.position.z);
        transform.LookAt(lookat);
        anim.SetBool("isPunching", true);
        IdlePunching();
    }

    void IdlePunching()
    {
        anim.SetBool("isIdlePunching", true);
    }

    #endregion

    #region Move
    void MoveTo(Transform pGameObject, float pSpeed)
    {
        CharacterController controller = this.GetComponent<CharacterController>();

        anim.SetFloat("InputMagnitude", pSpeed, 0.0f, Time.deltaTime);
        var forward = transform.TransformDirection(Vector3.forward);
        controller.SimpleMove(forward * pSpeed);
        Vector3 lookat = new Vector3(pGameObject.transform.position.x, gameObject.transform.position.y, pGameObject.transform.position.z);
        transform.LookAt(lookat);
    }

    void Chase()
    {
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "IdlePunching")
        {
            MoveTo(playerTarget.transform, pSpeed: 9f);
        }

    }

    public void Patrol(GameObject gameObject)
    {
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "IdlePunching")
        {
            MoveTo(gameObject.transform, pSpeed: 3f);
        }
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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

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

    public void PlaySoundEnnemis(AudioClip audioClip)
    {
        if (DistancePlayer < soundRange)
        {

            sounds.PlaySound(audioClip);
        }
    }

    public void TakeDammageInt(int dammageAmount)
    {
        if (pointsDeVie > 0)
        {
            enemiesHitPointManager.ModifyHealthWithValue(dammageAmount);
            pointsDeVie -= dammageAmount;
            MoveTo(playerTarget.transform, pSpeed: 9f);
        }
        else
        {
            Die();
        }

    }

    public void GiveDammage()
    {

        iDamageable dammagePlayer = playerTarget.GetComponent<iDamageable>();
        dammagePlayer.TakeDammageInt(DamageAmount);
    }
    private void Die()
    {

    }
    #endregion


}