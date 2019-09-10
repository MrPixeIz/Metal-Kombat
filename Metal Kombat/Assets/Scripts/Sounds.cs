using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {

    public AudioClip audioClip;
    public AudioSource audiosource;

    void Start()
    {
        audiosource.clip = audioClip;

    }

    void Update()
    {  
    }

    public void PlaySound(AudioClip clipAudio)
    {
        //audiosource.clip = clipAudio;
        
        audiosource.Play();
    }
}
