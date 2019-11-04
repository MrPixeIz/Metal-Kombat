using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlerSwitch : MonoBehaviour
{

    private Animator animation;
    private AudioSource electricSound;
    private AudioSource soundSwitch;
    private ChangeLightColor changelightColor;
    private bool EndGame = false;
    private float timeLeft = 5.0f;
    private DisplayEndMessage displayEndMessage;

    void Start()
    {

        animation = this.GetComponentInChildren<Animator>();

        AudioSource[] audios = GetComponents<AudioSource>();
        electricSound = audios[0];
        soundSwitch = audios[1];
        displayEndMessage = GetComponentInChildren<DisplayEndMessage>();
      
       

        changelightColor = GetComponentInChildren<ChangeLightColor>();
    }

    void Update()
    {
        if (EndGame == true)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void OnTriggerEnter()
    {
        animation.SetTrigger("TriggerSwitch");
        soundSwitch.Play();
    }

    public void TurnLight()
    {
        electricSound.Play();
        changelightColor.ChangeLightColorSwitch();
        displayEndMessage.DisplayMessage();
        EndGame = true;
    }

}
