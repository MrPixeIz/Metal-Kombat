using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiDetectionScript : MonoBehaviour
{
    public float inputX;
    public float inputZ;
    private float Distance;
    public Transform Target;
    public float chaseRange = 30;
    public float attackRange = 5;
    public Animator anim;
    public float speed;
    public Vector3 desiredMoveDirection;
    public float desiredRotationSpeed;

    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        Target = GameObject.Find("Player").transform;
        Distance = Vector3.Distance(Target.position, transform.position);

        if (Distance > chaseRange)
        {
            anim.SetBool("isPunching", false);
            anim.SetBool("isIdlePunching", false);
            Idle();
        }

        if (Distance <= chaseRange && Distance >= attackRange)
        {
            anim.SetBool("isPunching", false);
            anim.SetBool("isIdlePunching", false);
            Chase();
        }

        if (Distance <= attackRange)
        {
            Attack();
        }
    }

    void Chase()
    {
        CharacterController controller = this.GetComponent<CharacterController>();

        speed = 3f;

        anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
        var forward = transform.TransformDirection(Vector3.forward);
        controller.SimpleMove(forward * speed);
        transform.LookAt(Target);
    }

    void Idle()
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

}