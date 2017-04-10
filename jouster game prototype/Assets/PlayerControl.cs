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
    public Animator knightAnim;
    public Animator horseAnim;
    
    [SerializeField]
    private float knockbackpower = 2;

    private bool highKnockback = false;
    public float knockbackHeight = 10;
    public int player;
    int isAction;
    private bool inverted = false;
    public bool isStabbed = false;
    bool stunned = false;
    public float stunTimeInSeconds;
    float timeStampStun;
    public float shieldTime;
    float shieldTimeStamp;
    public bool shielded = false;
    public float highknockbackPower = 1.2f;
    public float clashPower = 0.2f;
    public float jabKnockback = 1.2f;
    float stunLength = 0f;
    float shieldLength = 0f;

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
        trans = transform;
        isAction = 0;
        if (player == 1)
        {
            inverted = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        enemy = other.gameObject.GetComponent<PlayerControl>();
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
                    highKnockback = true;
                }
                Clash();
            }
        }

    }

    private void FixedUpdate()
    {
        if (!stunned && trans.position.y >= 0.5f) Move();
    }

    // Update is called once per frame
    void Update()
    {

        //blockAnimation.Play(0);
        if (shielded)
        {
            shieldLength += Time.deltaTime;         
            print(shieldTime + " JAB ME!");
            if (shieldLength > shieldTime)
            {
                shielded = false;
                shieldLength = 0f;
            }
        }

        if (stunned)
        {
            stunLength += Time.deltaTime;
            if (stunLength > stunTimeInSeconds)
            {
                stunned = false;
                stunLength = 0f;
            }
        }

        #region Player Input
        if (Input.GetKeyUp(speedUp))
        {
            speed = speed + speedIncreace;
        }

        if (Input.GetKeyUp(speedDown))
        {
            speed = speed - speedIncreace;
        }
        
        if (Input.GetKeyUp(jabKey))
        {
            isAction = 1;
        }

        if (Input.GetKeyUp(parryKey))
        {
            isAction = 2;
        }

        #endregion

        if (!stunned)
        {
            switch (isAction)
            {
                case 1:
                    Jab();
                    break;
                    
                case 2:
                    Parry();
                    break;
                    
                case 3: 
                    Clash();
                    break;
                    
                default:
                    break;
            }
        }
    }

 

    private void Move()
    {
        if (inverted)
        {
            body.AddForce(Vector3.right * speed);
        }
        else
        {
            body.AddForce(Vector3.left * speed);
        }

    }

    private void Parry()
    {
        if (timeStampCooldownParry <= Time.time)
        {
            knightAnim.SetTrigger("Block");
            timeStampCooldownParry = Time.time + cooldownParryInSeconds;
            shieldTimeStamp = Time.time + shieldTime;
            shielded = true;
        }
        else
        {
            isAction = 0;
        }

    }

    private void Jab()
    {
        if (timeStampCooldownJab <= Time.time || trans.position.y   <=0.5)
        {
            print("stabcooldown started");
            knightAnim.SetTrigger("Jab");
            timeStampCooldownJab = Time.time + cooldownJabInSeconds;
            if (enemy != null)
            {
                print("jab enemy");
               
                if (enemy.shielded)
                {
                    print("STUN!");
                    stunned = true;
                    enemy.Clash();
                    knockbackpower = knockbackpower + jabKnockback;
                    Clash();
                }
                else
                {
                    enemy.isStabbed = true;
                    enemy.Clash();
                    enemy.knockbackpower = enemy.knockbackpower + jabKnockback;
                    Clash();
                }
            }
        }

        isAction = 0;
    }

    private void Clash()
    {
        knockbackpower = knockbackpower + clashPower;
        if (isStabbed)
        {
            //knockbackpower = knockbackpower * 1.2f;
            isStabbed = false;
            print(isStabbed);
            print("stabtest");
        }

        if (highKnockback)
        {
            print("highknockback calculated");
            enemy.knockbackpower = enemy.knockbackpower + highknockbackPower;
            highKnockback = false;
        }

        print(knockbackpower + " , playerNumber = " + player);
        body.AddForce(Vector3.right * ((inverted) ? -1 : 1) * knockbackpower);
        body.AddForce(Vector3.up * knockbackHeight);
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

