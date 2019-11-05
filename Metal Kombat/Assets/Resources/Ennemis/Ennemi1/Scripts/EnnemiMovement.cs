using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnnemiMovement : MonoBehaviour {

    public GameObject startPatrolObject;
    public GameObject endPatrolObject;

    protected Animator anim;
    protected GameObject playerTarget;
    protected GameObject patrolTarget;

    // Use this for initialization
    void Start () {
        patrolTarget = startPatrolObject;
        OnStart();
    }
	
	// Update is called once per frame
	void Update () {
        OnUpdate();
    }

    protected abstract void OnStart();
    protected abstract void OnUpdate();

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
        Vector3 lookat = new Vector3(playerTarget.transform.position.x, gameObject.transform.position.y, playerTarget.transform.position.z);
        transform.LookAt(lookat);
        anim.SetBool("isShooting", true);
        IdleShooting();
    }

    protected void IdleShooting()
    {
        anim.SetBool("isIdleShooting", true);
    }

    #endregion

    #region Move
    protected void MoveTo(Transform pGameObject, float pSpeed)
    {
        CharacterController controller = this.GetComponent<CharacterController>();

        anim.SetFloat("InputMagnitude", pSpeed, 0.0f, Time.deltaTime);
        var forward = transform.TransformDirection(Vector3.forward);
        controller.SimpleMove(forward * pSpeed);
        Vector3 lookat = new Vector3(pGameObject.transform.position.x, gameObject.transform.position.y, pGameObject.transform.position.z);
        transform.LookAt(lookat);
    }

    protected void Chase()
    {
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "IdlePunching")
        {
            MoveTo(playerTarget.transform, pSpeed: 15f);
        }

    }

    protected void Patrol(GameObject gameObject)
    {
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "IdlePunching")
        {
            MoveTo(gameObject.transform, pSpeed: 3f);
        }
    }

    protected void SetPatrol(GameObject newPatrolTarget)
    {
        patrolTarget = newPatrolTarget;
    }

    #endregion

    #region SetBool
    protected void ResetBool()
    {
        anim.SetBool("isPunching", false);
        anim.SetBool("isIdlePunching", false);
        anim.SetBool("isShooting", false);
    }

    #endregion
}
