using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEndMessage : MonoBehaviour {

    public Renderer renderer;
    private Text text;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        text = GetComponentInChildren<Text>();
        text.enabled = false;
    }

    public void DisplayMessage()
    {
        text.enabled = true;
    }
}
