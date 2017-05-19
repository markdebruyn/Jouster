﻿using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Other objects & Componentent")]
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private SkeletonAnimation skeletonAnimation;


    [Header("Cooldown & Stun")]
    [SerializeField] private float shieldAcitveTime;
    [SerializeField] private float cooldownShieldInSeconds;
    [SerializeField] private float stunTimeInSeconds;
    [SerializeField] private float cooldownJabInSeconds;
    private bool isOnCooldownShield = false;
    private bool isShielding = false;
    private bool isStunned = false;
    bool isOnCooldownJab = false;
    



    [Header("knockback power & Speed")]
    [SerializeField] private bool invertedMove = false;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float speedIncreace;
    [SerializeField] private float percentageSpeedDecrease;
    [SerializeField] private float baseKnockback;
    [SerializeField] private float lowerSpeedKnockbackIncrease;
    [SerializeField] private float higherSpeedKnockbackIncrease;
    [SerializeField] private float knockbackHeight = 5f;
    [SerializeField] private float clashPower;
    [SerializeField] private float jabPower;


    //[Header("AudioSources")]
    #region
    //public AudioSource audio;
    #endregion


    IEnumerator JabCooldown()
    {
        isOnCooldownJab = true;
        yield return new WaitForSeconds(cooldownJabInSeconds);
        isOnCooldownJab = false;
        yield break;
    }

    IEnumerator ShieldCooldown()
    {

        isOnCooldownShield = true;
        yield return new WaitForSeconds(cooldownShieldInSeconds);
        isOnCooldownShield = false;
        yield break;
    }

    IEnumerator ShieldActive()
    {
        isShielding = true;
        yield return new WaitForSeconds(shieldAcitveTime);
        isShielding = false;
        yield break;
    }

    IEnumerator StunActive()
    {
        isStunned = true;
        yield return new WaitForSeconds(stunTimeInSeconds);
        isStunned = false;
        yield break;
    }

    public void Move()
    {
        if (!isStunned)
        {
            if (isShielding)
            {
                body.AddForce(((invertedMove) ? Vector2.left : Vector2.right) * (speed * (percentageSpeedDecrease / 100)));
            }
            else
            {
                body.AddForce(((invertedMove) ? Vector2.left : Vector2.right) * speed);
            }
        }
    }

    public HitInfo retrieveHitInfo()
    {
        return new HitInfo(isShielding, isStunned, speed);
    }

    public void Shield()
    {
        if (!isOnCooldownShield)
        {
            //knightAnim.SetTrigger("Block");
            StartCoroutine("ShieldActive");
            print("isaction = 2 = shielding");
            StartCoroutine("ShieldCooldown");

        }
    }

    public void JabCooldownStart()
    {
        StartCoroutine("JabCooldown");
    }

    public void Jab(PlayerControllerInterface enemy)
    {
        if (!isOnCooldownJab)
            {
            //body.AddForce(Vector2.right * ((invertedMove) ? -1 : 1) * (baseKnockback));
            //body.AddForce(Vector2.up * knockbackHeight);
            enemy.GetJabbed(retrieveHitInfo());
        }
    }

    public void GetJabbed(float enemySpeed)
    {      
        if (enemySpeed > speed)
        {
            // bereknening hoge knockback
            baseKnockback = baseKnockback + higherSpeedKnockbackIncrease;
            KnockbackCalculation(jabPower);

        }
        else
        {
            // berekening lage knockback
            baseKnockback = baseKnockback + lowerSpeedKnockbackIncrease;
            KnockbackCalculation(jabPower);

        }
        StartCoroutine("StunActive");
    }

    public void Clash(HitInfo enemy)
    {
        
        if (enemy.speed > speed)
        {
            // bereknening hoge knockback
            baseKnockback = baseKnockback + higherSpeedKnockbackIncrease;
            KnockbackCalculation(clashPower);
        }
        else
        {
            // berekening lage knockback
            baseKnockback = baseKnockback + lowerSpeedKnockbackIncrease;
            KnockbackCalculation(clashPower);
        }
        if (isShielding)
        {
            print("stun in clash");
            StartCoroutine("StunActive");
        }
    }

    private void KnockbackCalculation(float knockback)
    {
        print("pushback");
        body.AddForce(((!invertedMove) ? Vector2.left : Vector2.right) * (baseKnockback + knockback));
        body.AddForce(Vector2.up * knockbackHeight);
    }
}

public struct HitInfo
{
    public bool isShielded;
    public bool stunned;
    public float speed;
    
    public HitInfo(bool isShield, bool isStun, float currentSpeed) {
        this.isShielded = isShield;
        this.stunned = isStun;
        this.speed = currentSpeed;
    }
}