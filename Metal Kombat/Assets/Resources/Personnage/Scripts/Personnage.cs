using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class Personnage : Physic
{
    public const float DESIREDROTATIONSPEED = 50;
    protected float pointsDeVie = 100;
    public GameObject raycastObject;
    public CharacterController controller;
    protected Sounds sounds;
    protected LifeBar barreDeVie; //= new LifeBar();
    protected Animator anim;
    protected GunBar barreGun;
    protected Camera cam;
    protected bool hasAGun = false;
    iDamageable damageable;
    protected Vector3 targetingVector = new Vector3(0, 0, 1);
    protected bool hasArmeInInventory = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        SetupLifeBar();
        SetOnDieEvent();
        OnStart();
    }
    protected abstract void SetupLifeBar();
    protected abstract void OnStart();
    protected abstract void Attack();
    protected abstract void Die();
    protected abstract void ApplyMoveInput();
    protected abstract void TakeDammage();
    protected abstract void ApplyMovement();
    protected override void ApplyVelocity()
    {
        ApplyMovement();
        controller.Move(velocity * Time.deltaTime);
    }



    public GameObject RaycastObject
    {
        get { return raycastObject; }
        set { raycastObject = value; }
    }

    protected bool MeleeHitCheck()
    {
        bool hitDetected = false;
        RaycastHit objectHit;
        //Debug.DrawRay(player.RaycastObject.transform.position + new Vector3(0, 5, 0), player.Fwd * 3, Color.green, 2);
        Physics.Raycast(raycastObject.transform.position, gameObject.transform.forward, out objectHit, 7);
        //Physics.SphereCast(transform.position + new Vector3(0, controller.height / 2, 0), controller.height / 2, transform.forward, out objectHit, 10)
        if (Physics.Raycast(RaycastObject.transform.position + new Vector3(0, 5, 0),
            gameObject.transform.forward, out objectHit, 3))
        {
            iDamageable ennemi = objectHit.collider.gameObject.GetComponent<iDamageable>();
            if (ennemi != null)
            {
                ennemi.TakeDammageInt(10);
            }
            hitDetected = true;
        }
        else
        {
            hitDetected = false;

        }
        return hitDetected;
    }

    private void SetOnDieEvent()
    {
        barreDeVie.SetOnDieListenner(GetOnDieEvent());
    }

    protected abstract OnDieHook GetOnDieEvent();

    public interface OnDieEvent
    {
        void OnDieEvent();
    }
    protected abstract class OnDieHook : OnDieEvent
    {
        public abstract void OnDieEvent();
    };
}
