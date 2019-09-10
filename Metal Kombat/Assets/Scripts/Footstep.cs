using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Footstep : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource source;

    

    void Start()
    {
        source.clip = audioClip;
       
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            source.Play();
        }
    }

    public void PlayFootstep()
    {
        source.Play();
    }
}
