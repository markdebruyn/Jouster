﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{

    private Rigidbody body;
    private Transform trans;
    private PlayerControl enemy;

    [SerializeField]
    private float knockbackpower = 2;
    public bool onLand = true;
    private bool highknockback = false;
    public float knockbackHight = 10;
    public int player;
    int isAction;
    private bool inverted = false;
    public bool isStabbed = false;
    bool stunned = false;
    


    int timecounter;
    float timeStampCooldownParry;
    float timeStampCooldownJab;
    public float cooldownJabInSeconds;
    public float cooldownParryInSeconds;
    public float speed;
    public float speedIncreace;

    [Header("input")]
    public KeyCode speedDown;
    public KeyCode speedUp;
    public KeyCode jabKey;
    public KeyCode parryKey;

   
    /*
    IEnumerator test()
    {
        yield return new  WaitForSecondsRealtime(2);
    }
    */

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
                Clash();
            }
        }

    }

    private void FixedUpdate()
    {
        if(trans.position.y >= 0.5f || !onLand) Move();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(speedUp))
        {
            speed = speed + speedIncreace;
        }
        if (Input.GetKeyUp(speedDown))
        {
            speed = speed - speedIncreace;
        }
        Lose();
        if (Input.GetKeyUp(jabKey))
        {
            isAction = 1;
        }
        if (Input.GetKeyUp(parryKey))
        {
            isAction = 2;
        }

   

        switch (isAction)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    jab();
                    break;
                }
            case 2:
                {
                    parry();
                    break;
                }
            case 3:
                {
                    Clash();
                    break;
                }
        }
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
        if (timeStampCooldownParry <= Time.time)
        {
            timeStampCooldownParry = Time.time + cooldownParryInSeconds;
            
        }
        else
        {
            isAction = 0;
        }

    }

    private void jab()
    {
        if (timeStampCooldownJab <= Time.time)
        {
            timeStampCooldownJab = Time.time + cooldownJabInSeconds;
            if (enemy != null)
            {
                enemy.isStabbed = true;
                enemy.Clash();
                enemy.knockbackpower = enemy.knockbackpower * 1.2f;
                Clash();
            }
            else
            {
                isAction = 0;
            }
        }
        else
        {
            isAction = 0;
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
