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
            if(isAPuncher)
            {
                ResetBool();
                Chase();
            }
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
        Ray sight = new Ray();
        sight.origin = new Vector3(transform.position.x, transform.position.y + 6.5f, transform.position.z);
        sight.direction = transform.forward;
        RaycastHit objectHit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(sight, out objectHit))
        {
            Debug.DrawLine(sight.origin, objectHit.point, Color.red,10);
            if (objectHit.collider.tag == "Player")
            {
                iDamageable playerDamage = objectHit.collider.gameObject.GetComponent<iDamageable>();
                playerDamage.TakeDammageInt(DamageAmount);
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