using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{

    private Rigidbody body;
    private Transform trans;
    private PlayerControl enemy;

    int timecounter;

    private Animator testanimator;

    public float speed;
    public float speedincreace;
    public KeyCode speedDown;
    public KeyCode speedUp;
    public KeyCode jumpkey;
    public KeyCode turnLeft;
    public KeyCode turnRight;
    public KeyCode jabKey;
    public KeyCode parryKey;
    public int player;
    int isAction;
    private bool inverted = false;    
    public bool isStabbed = false;
    float stunned = 0;

    [SerializeField]
    private float knockbackpower = 2;
    public bool onLand = true;
    public float jumpforce;
    private bool highknockback = false;
    public float knockbackHight = 5;

    // Use this for initialization
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        trans = gameObject.GetComponent<Transform>();
        isAction = 0;
        if (player == 1)
        {
            inverted = true;
        }
        testanimator = GetComponent<Animator>();
        
    }


    private void OnTriggerEnter(Collider other)
    {
        enemy = other.gameObject.GetComponent<PlayerControl>();
        print(enemy);
        print("enemy jabable");
        SphereCollider tefdtesvufw = new SphereCollider();
        if (other.GetType() == tefdtesvufw.GetType())
        {
            if (!enemy.onLand)
            {
                enemy.body.AddForce(Vector3.up * 5);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemy = null;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "enemy")
        {
            if (enemy != null)
            {
                print("knockback");
                if (enemy.speed < speed)
                {
                    highknockback = true;
                }
               // body.AddForce(Vector3.forward * 0);
                //dontmove = true;
                //Clash();
                isAction = 5;
            }
        }

    }

    private void FixedUpdate()
    {
        if(trans.position.y >= 0.5f || !onLand) Move();
    }

    void timekeeper()
    {
        
        print(stunned);
        timecounter++;
        if (stunned > 0)
        {
            stunned = stunned - Time.deltaTime;
            //isAction = 6;
        }
        else
        {
            stunned = 0;
            //isAction = 0;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        timekeeper();
        if (Input.GetKeyUp(speedUp))
        {
            speed = speed + speedincreace;
        }
        if (Input.GetKeyUp(speedDown))
        {
            speed = speed - speedincreace;
        }
        Lose();
        if (Input.GetKeyUp(jumpkey))
        {
            isAction = 1;
        }
        if (/*Input.GetKeyUp(turnLeft) || */(Input.GetKeyUp(turnRight)))
        {
            isAction = 2;
        }
        if (Input.GetKeyUp(jabKey))
        {
            isAction = 3;
        }
        if (Input.GetKeyUp(parryKey))
        {
            isAction = 4;
        }

   

        switch (isAction)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    jump();
                    break;
                }
            case 2:
                {
                    TurnLeft();
                    if (inverted)
                    {
                        trans.Rotate(new Vector3(0, 0, 0));
                    }
                    else 
                    {
                        trans.Rotate(new Vector3(0, 180, 0));                        
                    }
                    break;
                }
            case 3:
                {
                    jab();
                    break;
                }
            case 4:
                {
                    parry();
                    break;
                }
            case 5:
                {
                    Clash();
                    break;
                }
            case 6:
                {
                    break;
                }
        }
    }

    private void Clash()
    {
        knockbackpower = knockbackpower + 0.2f;  
        if (isStabbed)
        {
            //knockbackpower = knockbackpower * 1.2f;
            isStabbed = false;
            print(isStabbed);
            print("stabtest");
        }

        if (highknockback)
        {
            print("highknockback calculated");
            enemy.knockbackpower = enemy.knockbackpower + 1.2f;
            highknockback = false;
        }
        
        print("test");
        print(knockbackpower + " , playerNumber = " + player);
        body.AddForce(Vector3.right * ((inverted) ? -1 : 1) * knockbackpower);
        body.AddForce(Vector3.up * knockbackHight);
        isAction = 0;

    }

    private void Move()
    {
        float tempsspeed;
        if (!onLand)
        {
            tempsspeed = speed / 2;
        }
        else
        {
            tempsspeed = speed;
        }
        if (inverted)
        {
            body.AddForce(Vector3.right * tempsspeed);
        }
        else
        {
            body.AddForce(Vector3.left * tempsspeed);
        }

    }

    private void parry()
    {
        stunned = 1.5f;
        print("STUN");
        isAction = 0;
    }

    private void jab()
    {
        if (enemy != null)
        {
            enemy.isStabbed = true;
            enemy.isAction = 5;
            enemy.knockbackpower = enemy.knockbackpower * 1.2f;
            isAction = 5;         
        }
        else
        {
            isAction = 0;
        }
    }

    private void TurnLeft()
    {
        if (inverted)
        {
            trans.Rotate(new Vector3(0, 0, 0));
            inverted = false;
            
            isAction = 0;
        }
        else /*(!inverted)*/
        {
            trans.Rotate(new Vector3(0, 180, 0));
            inverted = true;
            
            isAction = 0;
        }
    }

    /*
    private void TurnRight()
    {
        if (!inverted)
        {
            trans.Rotate(new Vector3(0, 180, 0));
            inverted = true;
            isAction = 0;
        }
    }
    */

    private void jump()
    {
        if (isAction == 1)
        {
            if (onLand)
            {
                if (trans.position.y < 1.5)
                {
                    body.AddForce(Vector3.up * jumpforce);
                }
                else
                {
                    onLand = false;
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
                    onLand = true;
                    isAction = 0;
                }
            }
        }
    }

    private void Lose()
    {
        if (trans.position.y < 0)
        {
            if (trans.position.y < -20)
            {
                if (player == 1)
                {
                    SceneManager.LoadScene(3);
                }
                if (player == 2)
                {                    
                    SceneManager.LoadScene(2);
                }
            }
        }
    }

}
