using UnityEngine;

public class Back_To_Battle : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.JoystickButton1))
        {
            GameControl.Instance.NewRound();
        }
    }
}
