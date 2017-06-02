using UnityEngine;

public class start_Game : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //GameControl.Instance.ChangeToMenuScreen();
            GameControl.Instance.ChangeToBattleScreen();
        }
    }
}
