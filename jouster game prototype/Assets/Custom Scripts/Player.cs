using Spine.Unity;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Other objects & Componentent")]
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    [Header("Cooldown & Stun")]
    [SerializeField] private float shieldAcitveTimeInSeconds;
    [SerializeField] private float cooldownShieldInSeconds;
    [SerializeField] private float stunTimeInSeconds;
    [SerializeField] private float cooldownJabInSeconds;
    private bool isOnCooldownShield = false;
    private bool isShielding = false;
    private bool isStunned = false;
    private bool isOnCooldownJab = false;

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

    #region "Animations"
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
    [SerializeField] [SpineAnimation] private string ridder_idle = "Basis pose ridder";
    private string moveSpeedString;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float drafSpeed;
    [SerializeField] private float galopSpeed;
    #endregion


    //[Header("AudioSources")]
    #region
    //[SerializeField] private AudioSource audio;
    #endregion

    #region "IEnummerator"

    // Stop Coroutine is to stop all other instances of the same name IEnummerators.
    //This is done for buttonspamming to prevent effects stacking and other bugs.


    IEnumerator JabCooldown()
    {
        if (!isOnCooldownJab)
        {
            skeletonAnimation.state.SetAnimation(2, stoot, false);
            isOnCooldownJab = true;
        }        
        yield return new WaitForSeconds(cooldownJabInSeconds);
        isOnCooldownJab = false;
        skeletonAnimation.state.SetAnimation(2, ridder_idle, true);
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
        yield return new WaitForSeconds(shieldAcitveTimeInSeconds);
        skeletonAnimation.state.SetAnimation(2, ridder_idle, true);
        isShielding = false;
        StopCoroutine("ShieldActive");
        yield break;
    }

    IEnumerator StunActive()
    {
        print("STUNN");
        isStunned = true;
        skeletonAnimation.state.SetAnimation(2, ridder_idle, false);
        skeletonAnimation.state.SetAnimation(0, duizelig, true);
        yield return new WaitForSeconds(stunTimeInSeconds);
        skeletonAnimation.state.SetAnimation(0, duizelig, false); 
        isStunned = false;
        StopCoroutine("StunActive");
        yield break;
    }

    #endregion


    #region "Movement"

    public void SpeedChange(bool speedUp)
    {
        if (speedUp)
        {
            speed = speed + speedIncreace;
        }
        else
        {
            speed = speed - speedIncreace;
        }
        SpeedAnimationsChange();
    }

    private void SpeedAnimationsChange()
    {
        if (speed < drafSpeed && moveSpeedString != walk)
        {
            skeletonAnimation.state.SetAnimation(1, walk, true);
            moveSpeedString = walk;
        }
        else if (speed > walkSpeed && speed < galopSpeed && moveSpeedString != draf)
        {
            skeletonAnimation.state.SetAnimation(1, draf, true);
            moveSpeedString = draf;
        }
        else if (speed > galopSpeed && moveSpeedString != galop)
        {
            skeletonAnimation.state.SetAnimation(1, galop, true);
            moveSpeedString = galop;
        }
    }

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

    #endregion

    public HitInfo RetrieveHitInfo()
    {
        return new HitInfo(isShielding, speed);
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

    #region "Jab"

    public void JabCooldownStart()
    {
        StartCoroutine("JabCooldown");
    }

    public void Jab(PlayerControllerInterface enemy)
    {
        if (!isOnCooldownJab && !isShielding && !isStunned)
        {
            HitInfo enemyhitInfo = enemy.RetrieveHitInfo();
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

    public void GetJabbed(float enemySpeed)
    {
        StartCoroutine("StunActive");
        if (enemySpeed > speed)
        {
            // berekening hoge knockback
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

    #endregion

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
        body.AddForce(((!invertedMove) ? Vector2.left : Vector2.right) * (baseKnockback + knockback));
        body.AddForce(Vector2.up * knockbackHeight);
    }
}

public struct HitInfo
{
    public bool isShielded;
    public float speed;
    
    public HitInfo(bool isShield, float currentSpeed)
    {
        this.isShielded = isShield;
        this.speed = currentSpeed;
    }
}