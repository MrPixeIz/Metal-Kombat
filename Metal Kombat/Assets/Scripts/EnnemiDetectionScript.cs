using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiDetectionScript : MonoBehaviour
{
    public float inputX;
    public float inputZ;
    private float Distance;
    public Transform Target;
    public float chaseRange = 10;
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
            idle();
        }

        if (Distance <= chaseRange)
        {      
            chase();
        }
    }

    void chase()
    {
        /*Changer le inputX,Z pour qu'il prenne les coordonnées du Player*/
        inputX = 0;       
        inputZ = 0.2f;
        
        speed = 0.5f;

        anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
        transform.Translate(new Vector3(inputX, 0, inputZ));
    }

    void idle()
    {
        /*On met l'inputMagnitude à 0 pour que l'animation redevienne à Idle*/
        anim.SetFloat("InputMagnitude", 0, 0.0f, Time.deltaTime);
    }
}