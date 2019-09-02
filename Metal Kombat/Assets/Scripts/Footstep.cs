using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{

    public static AudioClip stepSound;
    static AudioSource source;

    void Start()
    {
        stepSound = Resources.Load<AudioClip>("pas");
        source = GetComponent<AudioSource>();
    }
    
    void Update()
    {
       
    }

    public static void PlaySound()
    {
        source.PlayOneShot(stepSound);
    }

}
