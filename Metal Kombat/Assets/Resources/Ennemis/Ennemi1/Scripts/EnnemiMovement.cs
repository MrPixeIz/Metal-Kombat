using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnnemiMovement : Physic
{

    public GameObject startPatrolObject;
    public GameObject endPatrolObject;

    protected Animator anim;
    protected GameObject playerTarget;
    protected GameObject patrolTarget;

    private int timeBeforeLookAt = 30;
    private CharacterController controller;
    private bool isChasing = false;

    // Use this for initialization
    void Start()
    {
        
        controller = this.GetComponent<CharacterController>();
        patrolTarget = startPatrolObject;
        OnStart();
    }

    // Update is called once per frame
    /*void Update()
    {
        OnUpdate();
    }*/

    protected abstract void OnStart();
   // protected abstract void OnUpdate();

    #region Movement
    protected void Idle()
    {
        anim.SetFloat("InputMagnitude", 0, 0.0f, Time.deltaTime);
    }

    protected void Attack()
    {
        Vector3 lookat = new Vector3(playerTarget.transform.position.x, gameObject.transform.position.y, playerTarget.transform.position.z);
        transform.LookAt(lookat);
        anim.SetBool("isPunching", true);
        IdlePunching();
    }

    protected void IdlePunching()
    {
        anim.SetBool("isIdlePunching", true);
    }

    protected void Shoot()
    {
        timeBeforeLookAt--;
        if(timeBeforeLookAt == 0)
        {
            Vector3 lookat = new Vector3(playerTarget.transform.position.x, playerTarget.transform.position.y, playerTarget.transform.position.z);
            transform.LookAt(lookat);
            timeBeforeLookAt = 30;
        }
        anim.SetBool("isShooting", true);
        ReShooting();
    }

    protected void ReShooting()
    {
        anim.SetBool("isReShooting", true);
    }

    #endregion

    #region Move
    protected void MoveTo(Transform pGameObject, float pSpeed)
    {
        if (isChasing)
            pGameObject = playerTarget.transform;
        if (pSpeed > 0.3f)
            pSpeed = 0.3f;
        anim.SetFloat("InputMagnitude", pSpeed, 0.0f, Time.deltaTime);
        var forward = transform.TransformDirection(Vector3.forward);
        forward += velocity;
        controller.Move(forward * pSpeed);
        Vector3 lookat = new Vector3(pGameObject.transform.position.x, gameObject.transform.position.y, pGameObject.transform.position.z);
        transform.LookAt(lookat);
    }

    protected void Chase()
    {
        isChasing = true;
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "IdlePunching")
        {
            MoveTo(playerTarget.transform, pSpeed: 0.3f);
        }

    }

    protected void Patrol(GameObject gameObject)
    {
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "IdlePunching")
        {
            MoveTo(gameObject.transform, pSpeed: 0.1f);
        }
    }

    public void SetPatrol(GameObject newPatrolTarget)
    {
        patrolTarget = newPatrolTarget;
    }

    #endregion
    protected override void ApplyVelocity()
    {
    }
    #region SetBool
    protected void ResetBool()
    {
        isChasing = false;
        anim.SetBool("isPunching", false);
        anim.SetBool("isIdlePunching", false);
        anim.SetBool("isShooting", false);
        anim.SetBool("isReShooting", false);
    }

    #endregion
}
