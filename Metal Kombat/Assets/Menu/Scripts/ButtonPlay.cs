using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonPlay : MonoBehaviour {
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void PlayLevelOne()
    {
        SceneManager.LoadScene("mapLevel1");
    }
    public void BackToMenu()
    {
 
        SceneManager.LoadScene("menu");
    }
    public void GoToAbout()
    {
        SceneManager.LoadScene("About");
    }


}
