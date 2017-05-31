using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCommand : MonoBehaviour {

    private Movement controlClass;
    private Rigidbody body;
    private Transform trans;
    private bool inverted = false;

    // Use this for initialization
    void Start ()
    {
        controlClass = (Movement)GetComponent("Movement");
        body = gameObject.GetComponent<Rigidbody>();
        trans = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (inverted)
            {
                trans.Rotate(new Vector3(0, 0, 0));
                // reverse speed
                // return command reverse speed
            }
            else
            {
                trans.Rotate(new Vector3(0, 180, 0));
                // reverse speed
                // return command reverse speed
            }
        }
	}
}
