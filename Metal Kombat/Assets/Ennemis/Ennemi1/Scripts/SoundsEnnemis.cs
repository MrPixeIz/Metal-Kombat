using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsEnnemis : MonoBehaviour {

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
        audiosource.clip = clipAudio;
        audiosource.volume = Random.Range(0.4f, 0.5f);
        audiosource.pitch = Random.Range(0.9f, 1.3f);
        audiosource.Play();
    }
    
}
