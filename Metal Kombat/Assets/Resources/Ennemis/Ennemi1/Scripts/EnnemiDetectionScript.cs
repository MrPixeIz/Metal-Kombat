using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnnemiDetectionScript : EnnemiMovement, iDamageable
{
    public bool isAPuncher;
    public GameObject gun;

    private float DistancePlayer;
    private float chaseRange = 60;
    private float meleeAttackRange = 5;
    private float gunAttackRange = 30;
    private float soundRange = 60;
    private Sounds sounds;
    private float pointsDeVie = 100;
    private EnemiesHitPointManager enemiesHitPointManager;

    public int DamageAmount
    {
        get
        {
            if(isAPuncher)
            {
                return 10;
            }
            else
            {
                return 20;
            }
        }

        set
        {
            DamageAmount = value;
        }
    }


    protected override void OnStart()
    {
        anim = this.GetComponent<Animator>();
        sounds = GetComponentInChildren<Sounds>();
        enemiesHitPointManager = GetComponentInChildren<EnemiesHitPointManager>();
        if (isAPuncher)
        {
            anim.SetBool("isAPuncher", isAPuncher);
            gun.SetActive(false);
        }
        else
        {
            anim.SetBool("isAShooter", !isAPuncher);
            gun.SetActive(true);
        }
    }

    protected override void OnUpdate()
    {
        playerTarget = GameObject.Find("Player");
        DistancePlayer = Vector3.Distance(playerTarget.transform.position, transform.position);

        if (pointsDeVie == 100)
        {
            VerifyIfHitSomething();
            if(isAPuncher)
            {
                if (DistancePlayer <= chaseRange && DistancePlayer >= meleeAttackRange)
                {
                    ResetBool();
                    Chase();
                }
            }
            else
            {
                if (DistancePlayer <= chaseRange && DistancePlayer > gunAttackRange)
                {
                    ResetBool();
                    Chase();
                }
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

        if (DistancePlayer <= meleeAttackRange && isAPuncher)
        {
            Attack();
        }

        if (DistancePlayer <= gunAttackRange && !isAPuncher)
        {
            Shoot();
        }
    }



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

    public void VerifyIfBulletHit()
    {
        RaycastHit objectHit;
        Vector3 targetingVector = new Vector3(0,0,1);

        if (Physics.Raycast(transform.position, playerTarget.transform.position, out objectHit))
        {
            Vector3 bulletImpactLocation = objectHit.point;
            targetingVector = (bulletImpactLocation - (gameObject.transform.position + new Vector3(0.5f, 7.5f, 0)));
            Debug.DrawRay(gameObject.transform.position + new Vector3(0.5f, 7.5f, 0), targetingVector, Color.blue, 10);
            iDamageable player = objectHit.collider.gameObject.GetComponent<iDamageable>();
            if (player != null)
            {
                player.TakeDammageInt(DamageAmount);
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