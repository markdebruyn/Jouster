using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float speed;
    private Rigidbody body;
    private Transform trans;
    Vector3 movement;
    public float speedincreace;
    public bool crashed;
    public float crashSpeedIncrease;
    public KeyCode key;
    public KeyCode jumpkey;
    public bool left;
    public int player;
    Movement enemyMovement;

    // Use this for initialization
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        trans = gameObject.GetComponent<Transform>();
    }



    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            //enemyMovement = (Movement)other.gameObject;

           /* if (left)
            {
                speedincreace = 0;
                crashSpeedIncrease = 0;
                speed = 0;
                body.AddForce(Vector3.left * 10000);
            }
            else
            {
                speedincreace = 0;
                crashSpeedIncrease = 0;
                speed = 0;
                body.AddForce(Vector3.right * 10000);
            }*/
            crashed = true;
            if (other.transform.position.y < 0)
            {                
                speed = 0;
                speedincreace = 0;
                crashSpeedIncrease = 0;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        {
            if (trans.position.y < 0)
            {
                /*enemyMovement.speed = 0;
                enemyMovement.speedincreace = 0;
                enemyMovement.crashSpeedIncrease = 0;*/
                if (trans.position.y < -20)
                {
                    if (player == 1)
                    {
                        SceneManager.LoadScene(2);                        
                    }
                    if (player == 2)
                    {
                        SceneManager.LoadScene(3);
                    }
                }
            }
            else
            {
                if (crashed)
                {
                    if (Input.GetKeyUp(key))
                    {
                        speed = speed + crashSpeedIncrease;
                    }
                }
                else
                {
                    if (Input.GetKeyUp(key))
                    {
                        speed = speed + speedincreace;
                    }
                }
                if (left)
                {
                    body.velocity = Vector3.left * speed;
                }
                else
                {
                    body.velocity = Vector3.right * speed;
                }
                
            }
        }
    }
}
