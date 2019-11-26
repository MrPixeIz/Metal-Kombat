using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnnemiDetectionScript : EnnemiMovement, iDamageable
{
    public bool isAPuncher;
    public GameObject gun;
    public GameObject laserPrefab;

    private float DistancePlayer;
    private Sounds sounds;
    private float pointsDeVie = 100;
    private EnemiesHitPointManager enemiesHitPointManager;

    public AudioClip audioClip;
    public AudioSource audiosource;
    GameObject instance;
    public GameObject lightning;
    public int DamageAmount
    {
        get
        {
            if (isAPuncher)
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
        audiosource.clip = audioClip;
        playerTarget = GameObject.Find("Player");
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
        float chaseRange = 60;
        float meleeAttackRange = 5;
        float gunAttackRange = 30;

        DistancePlayer = Vector3.Distance(playerTarget.transform.position, transform.position);

        if (pointsDeVie == 100)
        {
            VerifyIfHitSomething();
            if (isAPuncher)
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
            if (isAPuncher)
            {
                ResetBool();
                Chase();
            }
            else
            {
                if(DistancePlayer > gunAttackRange)
                {
                    ResetBool();
                    Chase();
                }
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
            Debug.DrawLine(sight.origin, objectHit.point, Color.red, 10);
            ShootLaserFromTargetPosition(sight.origin);
            if (objectHit.collider.tag == "Player")
            {
                iDamageable playerDamage = objectHit.collider.gameObject.GetComponent<iDamageable>();
                playerDamage.TakeDammageInt(DamageAmount);
            }
        }
    }

    void ShootLaserFromTargetPosition(Vector3 targetPosition)
    {
        GameObject laserGO = Instantiate(laserPrefab, targetPosition, gameObject.transform.rotation);
    }

    public void PlaySoundEnnemis(AudioClip audioClip)
    {
        float soundRange = 60;
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
            lightningFX();
        }

    }
    private void lightningFX()
    {
        instance = Instantiate(lightning, this.transform.position + new Vector3(0, 5, 0), new Quaternion(90, 0, 0, 0)) as GameObject;
        PlaySound(audioClip);

    }
    public void PlaySound(AudioClip clipAudio)
    {
        //audiosource.clip = clipAudio;
        audiosource.volume = Random.Range(0.4f, 0.5f);
        audiosource.pitch = Random.Range(0.9f, 1.3f);
        audiosource.PlayOneShot(clipAudio);
    }
    public void GiveDammage()
    {
        iDamageable dammagePlayer = playerTarget.GetComponent<iDamageable>();
        dammagePlayer.TakeDammageInt(DamageAmount);
    }

    #endregion


}