using UnityEngine;

public class PlayerControl : MonoBehaviour, PlayerControllerInterface
{
    [SerializeField] private Player player;
    private bool start = false;

    public GameObject enemy;
    public int playerID;
    private int isAction;
    private bool enemyInRange = false;
    
    [Header("input")]
    public KeyCode speedDown;
    public KeyCode speedUp;
    public KeyCode jabKey;
    public KeyCode parryKey;

    System.Collections.IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1);

        yield return new WaitForSeconds(1);

        yield return new WaitForSeconds(1);
        start = true;
        // Dirty way of starting animation, when continues input is a fact this is unneeded.
        player.SpeedChange(true);
        player.SpeedChange(false);
        yield break;
    } 

    // Use this for initialization
    void Start()
    {
        StartCoroutine("CountDown");
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
            collision.gameObject.GetComponent<PlayerControllerInterface>().Clash(player.RetrieveHitInfo());
        }
    }


    private void FixedUpdate()
    {
        if (start)
        {
            player.Move();
        }
    }

    // so the other player can get this players HitInfo during a hit
    public HitInfo RetrieveHitInfo()
    {
        return player.RetrieveHitInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
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
        }

        #endregion
        // old code, still works but consider using an if statement instead of a switch
        // cannot be placed instead of the isAction number change because that would mean multiple input would be possible... potentonally
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
    
    public void GetJabbed(HitInfo hitInfo)
    {
        HitInfo playerInfo = player.RetrieveHitInfo();
        if (playerInfo.isShielded)
        {
            enemy.GetComponent<PlayerControllerInterface>().GetJabbed(playerInfo);
        }
        else
        {
            player.GetJabbed(hitInfo.speed);
        }
    }

    public void Clash(HitInfo hitInfo)
    {
        player.Clash(hitInfo);
    }
}

