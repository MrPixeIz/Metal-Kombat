using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class Personnage : Physic
{
    public const float DESIREDROTATIONSPEED = 50;
    public GameObject raycastObject;
    public CharacterController controller;
    protected Sounds sounds;
    protected LifeBar barreDeVie;
    protected Animator anim;
    protected GunBar barreGun;
    protected Camera cam;
    protected bool hasAGun = false;
    protected Vector3 targetingVector = new Vector3(0, 0, 1);
    protected Vector3 lookAt = new Vector3(0, 8, 5);
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

    protected abstract void TakeDammage(int damage);

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
        Physics.Raycast(raycastObject.transform.position, gameObject.transform.forward, out objectHit, 7);
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
