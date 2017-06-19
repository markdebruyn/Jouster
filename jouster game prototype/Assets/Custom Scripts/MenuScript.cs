
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.JoystickButton1) || Input.GetKeyUp(KeyCode.B))
        {
            GameControl.Instance.ChangeToTutorialScreen();
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton0) || Input.GetKeyUp(KeyCode.A))
        {
            GameControl.Instance.ChangeToBattleScreen();
        }
        // test in a build not in unity play mode
        if (Input.GetKeyUp(KeyCode.JoystickButton3) || Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Y))
        {
            Application.Quit();
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton2) || Input.GetKeyUp(KeyCode.X))
        {
            GameControl.Instance.ChangeToCreditsScreen();
        }
    }
}
