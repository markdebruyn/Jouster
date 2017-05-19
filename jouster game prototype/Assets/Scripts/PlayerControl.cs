using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour, PlayerControllerInterface
{
    [SerializeField] public Player player;


    public GameObject enemy;
    public int playerID;
    int isAction;
    bool enemyInRange = false;
    
    

    bool stunned = false;

    public float stunTimeInSeconds;
    float timeStampStun;




    //int timecounter;
    //public float cooldownJabInSeconds;

    [Header("input")]
    public KeyCode speedDown;
    public KeyCode speedUp;
    public KeyCode jabKey;
    public KeyCode parryKey;


    // Use this for initialization
    void Start()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetType() == typeof(BoxCollider2D))
        {
            enemyInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetType() == typeof(BoxCollider2D))
        {
            enemyInRange = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != tag && collision.gameObject.tag != "Untagged")
        {
            collision.gameObject.GetComponent<PlayerControllerInterface>().Clash(player.retrieveHitInfo());
        }
    }


    private void FixedUpdate()
    {
        player.Move();
       
       // if (!stunned && trans.position.y >= 0.5f) player.Move();
    }

    // Update is called once per frame
    void Update()
    {
        /*
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
        */
        #region Player Input
        if (Input.GetKeyUp(speedUp))
        {
            player.SpeedChange(true);
        }

        if (Input.GetKeyUp(speedDown))
        {
            player.SpeedChange(false);
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
                    
                    if (enemyInRange)
                    {
                        player.Jab(enemy.GetComponent<PlayerControllerInterface>());
                    }
                    player.JabCooldownStart();
                    isAction = 0;
                    break;
                    
                case 2:
                    player.Shield();
                    isAction = 0;
                    break;
                    
                default:
                    break;
            }
        }
    }

    public void GetJabbed(HitInfo hitInfo)
    {
        HitInfo playerInfo = player.retrieveHitInfo();
        if (playerInfo.isShielded)
        {
            enemy.GetComponent<PlayerControllerInterface>().GetJabbed(playerInfo);
        } else {
            player.GetJabbed(hitInfo.speed);
        }
    }

    public void Clash(HitInfo hitInfo)
    {
        player.Clash(hitInfo);
    }
}

