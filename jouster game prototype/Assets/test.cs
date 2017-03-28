using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    private Movement playerStatus;

	// Use this for initialization
	void Start ()

    {
        playerStatus = (Movement)GetComponent("Movement");        
    }
	
	// Update is called once per frame
	void Update ()
    {
        playerStatus.speedincreace = playerStatus.speedincreace + 0.01f;		
	}
}
