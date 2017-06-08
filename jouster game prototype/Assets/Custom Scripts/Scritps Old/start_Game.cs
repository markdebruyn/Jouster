using UnityEngine;

public class start_Game : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)|| Input.GetKeyUp(KeyCode.JoystickButton1))
        {
            GameControl.Instance.ChangeToMenuScreen();
        }
    }
}
