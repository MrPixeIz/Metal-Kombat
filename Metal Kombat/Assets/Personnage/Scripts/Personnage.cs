using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class Personnage : Physic
{
    public const float DESIREDROTATIONSPEED = 50;
    int pointsDeVie;
    public GameObject raycastObject;
    public CharacterController controller;
    protected  Sounds sounds;
    public int PointsDeVie
    {
        get { return pointsDeVie; }
        set { pointsDeVie = value; }
    }

    public Personnage()
    {
        pointsDeVie = 0;
    }
    public Personnage(int inPointsDeVie)
    {
        pointsDeVie = inPointsDeVie;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
    }

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
        //Physics.Raycast(raycastObject.transform.position, fwd, out objectHit, 7
        //Physics.SphereCast(transform.position + new Vector3(0, controller.height / 2, 0), controller.height / 2, transform.forward, out objectHit, 10)
        if (Physics.Raycast(RaycastObject.transform.position + new Vector3(0, 5, 0),
            gameObject.transform.forward, out objectHit, 3))
        {
            hitDetected = true;
        }
        else
        {
            hitDetected = false;

        }
        return hitDetected;
    }

    
   
}
