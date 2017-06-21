using UnityEngine;

public class start_Game : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)|| Input.GetKeyUp(KeyCode.JoystickButton0) || Input.GetKeyUp(KeyCode.JoystickButton1) || Input.GetKeyUp(KeyCode.JoystickButton2) || Input.GetKeyUp(KeyCode.JoystickButton3) || Input.GetKeyUp(KeyCode.Y)|| Input.GetKeyUp(KeyCode.A))
        {
            GameControl.Instance.ChangeToMenuScreen();
        }
    }
}
