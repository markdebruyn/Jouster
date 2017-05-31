using UnityEngine;

public class movement2 : MonoBehaviour
{
    public float speed;
    private Rigidbody body;
    private Transform trans;
    Vector3 movement;
    public float speedincreace;
    public bool crashed;
    public float crashSpeedIncrease;

    // Use this for initialization
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        trans = gameObject.GetComponent<Transform>();
        //position = trans.position 
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
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
                speed = 0;
                if (trans.position.y > -20)
                {

                }
            }
            else
            {
                if (crashed)
                    if (Input.GetKeyUp(KeyCode.Keypad8))
                    {
                        speed = speed + crashSpeedIncrease;
                    }
            
                    else
                {
                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        speed = speed + speedincreace;
                    }
                }
                body.velocity = Vector3.left * speed;
            }
        }
    }
    
}
