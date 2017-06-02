using UnityEngine;

public class Back_To_Start : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GameControl.Instance.NewRound();
        }
    }
}
