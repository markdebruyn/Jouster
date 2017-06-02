using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameControl : MonoBehaviour
{
    #region 
    /*
     This Region is what makes this class a Singleton. 
     The padlock is to prevent multiple treads from potentionally creating another instance.
     
    */
    private static GameControl instance = null;
    private static readonly object padlock = new object();

    public static GameControl Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new GameControl();
                }
                return instance;
            }
        }
    }
    #endregion

    private enum ScenesEnum
    {
        StartScreen = 0,
        MenuScreen = 1,
        TutorialScreen = 2,
        BattleScreen = 3,
        Player1Lost = 4,
        Player2Lost = 5,
        EndScreen = 6,
    }

    // To make sure the values are not reset after scene schange
    static int gameRound = 0;
    static int blueMatchWins = 0;
    static int redMatchWins = 0;
    static int gameWinsBlue = 0;
    static int gameWinsRed = 0;
    // change does not work 
    static int amountOfMatches = 3;

    void Start()
    {
        // On load all non-static variables will be reset!
        DontDestroyOnLoad(this);
    }

    public void Lose(int loser)
    {
        if (loser == 1)
        {
            RedWin();
        }
        else if (loser == 2)
        {
            BlueWin();
        } 
    }

    private void BlueWin()
    {
        blueMatchWins++;
        if ((blueMatchWins + redMatchWins) == amountOfMatches)
        {
            GameWin();
        }
        else
        {
            SceneManager.LoadScene((int)(ScenesEnum.Player1Lost));
        }
    }

    private void RedWin()
    {
        redMatchWins++;
        if ((blueMatchWins + redMatchWins) == amountOfMatches)
        {
            GameWin();
        }
        else
        {
            SceneManager.LoadScene((int)(ScenesEnum.Player2Lost));
        }
    }

    private void GameWin()
    {
        
        if (blueMatchWins > redMatchWins)
        {
            gameWinsBlue++;
        }
        else
        {
            gameWinsRed++;
        }
        SceneManager.LoadScene((int)(ScenesEnum.EndScreen));
    }

    public void NewRound()
    {
        SceneManager.LoadScene((int)(ScenesEnum.BattleScreen));
    }

    public void ChangeToBattleScreen()
    {
        SceneManager.LoadScene((int)(ScenesEnum.BattleScreen));
    } 

    public void ChangeToMenuScreen()
    {
        SceneManager.LoadScene((int)(ScenesEnum.MenuScreen));
    } 

    public void ChangeToTutorialScreen()
    {
        SceneManager.LoadScene((int)(ScenesEnum.TutorialScreen));
    }
}
