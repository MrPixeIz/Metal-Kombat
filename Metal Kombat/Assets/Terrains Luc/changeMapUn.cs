using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class changeMapUn : MonoBehaviour
{

    void OnTriggerEnter()
    {
        SceneManager.LoadScene(3);
    }

}
