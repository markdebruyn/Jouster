using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    int gameRound;
    int blueMatchWins;
    int redMatchWins;
    int gameWinsBlue;
    int gameWinsRed;
    [SerializeField] int amountOfMatches = 3;

    enum ScenesEnum
    {
        StartScreen = 0,
        BattleScreen= 1,
        Player1Lost = 2,
        Player2Lost = 3,
        EndScreen   = 4,
        TutorialScreen = 5,
    }

    void Lose(int loser)
    {
        if (loser == 1)
        {
            blueMatchWins++;
            MatchEnd();
            SceneManager.LoadScene((int)(ScenesEnum.Player1Lost));
        }
        else if (loser == 2)
        {
            redMatchWins++;
            MatchEnd();
            SceneManager.LoadScene((int)(ScenesEnum.Player2Lost));
        }
    }

    private void MatchEnd()
    {
        if (blueMatchWins + redMatchWins == 3)
        {
            SceneManager.LoadScene((int)(ScenesEnum.EndScreen));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int loser = collision.gameObject.GetComponent<PlayerControl>().playerID;
        Lose(loser);
    }

    public void ChangeToTutorialScreen()
    {
        SceneManager.LoadScene((int)(ScenesEnum.TutorialScreen));
    }


    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(this);
        print("will not be destroyed");
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}
}
