using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteLightning : MonoBehaviour {
    GameObject instance;
    private float targetTime;
    public float timetoLight;
    // Use this for initialization
    void Start () {
        timetoLight = 0.2f;
        targetTime = timetoLight;
    }
	
	// Update is called once per frame
	void Update () {
        targetTime -= Time.deltaTime;

        this.transform.Rotate(0, 0, 0);

        if (targetTime <= 0.0f)
        {
            targetTime = timetoLight;
            Destroy(gameObject);
        }
    }
}
