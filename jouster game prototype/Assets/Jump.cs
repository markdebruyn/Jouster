using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{

    private Movement controlClass;
    private Rigidbody body;
    private Transform trans;
    public bool up = true;
    public bool jump = false;
    public float jumpforce;

    // Use this for initialization
    void Start()
    {
        controlClass = (Movement)GetComponent("Movement");
        body = gameObject.GetComponent<Rigidbody>();
        trans = gameObject.GetComponent<Transform>();

    }

    public void jumping()
    {
        if (Input.GetKeyUp(controlClass.jumpkey))
        {
            jump = true;
        }
        if (jump)
        {
            if (up)
            {
                if (trans.position.y < 2.5)
                {
                    body.AddForce(Vector3.up * jumpforce);
                }
                else
                {
                    up = false;
                }
            }
            else
            {
                if (trans.position.y > 0.51)
                {
                    body.AddForce(Vector3.down * jumpforce);
                }
                else
                {
                    up = true;
                    jump = false;
                }
            }
        }
    }
        
        // Update is called once per frame
        void Update()
    {
        jumping();
    }
}
