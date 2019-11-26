using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class changeMapUn : MonoBehaviour
{

    void OnTriggerEnter()
    {
        //DontDestroyOnLoad(GameObject.Find("Personnage"));
        SceneManager.LoadScene(3);
    }

}
