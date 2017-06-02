using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool isOnCooldownShield = false;
    public bool isShielding = false;
    public bool isStunned = false;
    public bool isOnCooldownJab = false;
    



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

    [Header("SpineAnimaties")]
    [SerializeField] [SpineAnimation] private string idle = "0_Idle";
    [SerializeField] [SpineAnimation] private string walk = "1_Stap";
    [SerializeField] [SpineAnimation] private string draf = "2_Draf";
    [SerializeField] [SpineAnimation] private string galop = "3_Galop";
    [SerializeField] [SpineAnimation] private string duizelig = "Duizelig";
    [SerializeField] [SpineAnimation] private string hoge_terugslag = "Hoge_terugslag";
    [SerializeField] [SpineAnimation] private string lage_terugslag = "Lage_terugslag";
    [SerializeField] [SpineAnimation] private string land_Animation = "Land animation";
    [SerializeField] [SpineAnimation] private string schild_afweer = "Schild_afweer";
    [SerializeField] [SpineAnimation] private string schild_loop = "Schild_loop";
    [SerializeField] [SpineAnimation] private string stoot = "Stoot";
    [SerializeField] [SpineAnimation] private string ridderIdle = "Basis pose ridder";
    private string moveSpeedString;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float drafSpeed;
    [SerializeField] private float gallopSpeed;

    //[Header("AudioSources")]
    #region
    //[SerializeField] private AudioSource audio;
    #endregion


    IEnumerator JabCooldown()
    {
        if (!isOnCooldownJab)
        {
            skeletonAnimation.state.SetAnimation(2, stoot, false);
            isOnCooldownJab = true;
        }        
        yield return new WaitForSeconds(cooldownJabInSeconds);
        isOnCooldownJab = false;
        skeletonAnimation.state.SetAnimation(2, ridderIdle, true);
        StopCoroutine("JabCooldown");
        yield break;
    }

    IEnumerator ShieldCooldown()
    {
        isOnCooldownShield = true;
        yield return new WaitForSeconds(cooldownShieldInSeconds);
        isOnCooldownShield = false;
        StopCoroutine("ShieldCooldown");
        yield break;
    }

    IEnumerator ShieldActive()
    {
        isShielding = true;
        skeletonAnimation.state.SetAnimation(2, schild_loop, true);
        yield return new WaitForSeconds(shieldAcitveTime);
        skeletonAnimation.state.SetAnimation(2, ridderIdle, true);
        isShielding = false;
        StopCoroutine("ShieldActive");
        yield break;
    }

    IEnumerator StunActive()
    {
        print("STUNN");
        isStunned = true;
        skeletonAnimation.state.SetAnimation(2, ridderIdle, false);
        skeletonAnimation.state.SetAnimation(0, duizelig, true);
        yield return new WaitForSeconds(stunTimeInSeconds);
        skeletonAnimation.state.SetAnimation(0, duizelig, false); 
        isStunned = false;
        StopCoroutine("StunActive");
        yield break;
    }
    // animation does not work when starting all the time.
    public void Move()
    {
        if (!isStunned)
        {
            if (isShielding)
            {                
                skeletonAnimation.state.SetAnimation(2, schild_loop, false);
                body.AddForce(((invertedMove) ? Vector2.left : Vector2.right) * (speed * (percentageSpeedDecrease / 100)));
            }
            else
            {                
                body.AddForce(((invertedMove) ? Vector2.left : Vector2.right) * speed);
            }
        }
    }

    public HitInfo RetrieveHitInfo()
    {
        return new HitInfo(isShielding, isStunned, speed);
    }

    public void Shield()
    {
        if (!isOnCooldownShield && !isStunned)
        {
            //knightAnim.SetTrigger("Block");
            StartCoroutine("ShieldActive");
            StartCoroutine("ShieldCooldown");

        }
    }

    public void JabCooldownStart()
    {
        StartCoroutine("JabCooldown");
    }

    public void Jab(PlayerControllerInterface enemy)
    {
        if (!isOnCooldownJab && !isShielding && !isStunned)
        {
            HitInfo enemyhitInfo = enemy.retrieveHitInfo();
            if (enemyhitInfo.isShielded)
            {
                GetJabbed(enemyhitInfo.speed);
            }
            else
            {
                enemy.GetJabbed(RetrieveHitInfo());
            }
        }
    }

    public void SpeedChange(bool speedUp)
    {
        if (speedUp)
        {
            speed = speed + speedIncreace;
            SpeedAnimationsChange();
        }
        else
        {            
            speed = speed - speedIncreace;
            SpeedAnimationsChange();
        }
    }

    private void SpeedAnimationsChange()
    {
        if (speed < drafSpeed && moveSpeedString != walk)
        {
            skeletonAnimation.state.SetAnimation(1, walk, true);
            moveSpeedString = walk;
        }
        else if (speed > walkSpeed && speed < gallopSpeed && moveSpeedString != draf)
        {
            skeletonAnimation.state.SetAnimation(1, draf, true);
            moveSpeedString = draf;
        }
        else if (speed > gallopSpeed && moveSpeedString != galop)
        {
            skeletonAnimation.state.SetAnimation(1, galop, true);
            moveSpeedString = galop;
        }
    }

    public void GetJabbed(float enemySpeed)
    {
        StartCoroutine("StunActive");
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
    
    public HitInfo(bool isShield, bool isStun, float currentSpeed)
    {
        this.isShielded = isShield;
        this.stunned = isStun;
        this.speed = currentSpeed;
    }
}