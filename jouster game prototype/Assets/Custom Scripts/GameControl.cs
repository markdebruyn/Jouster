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
     Essentailly makes sure the can only be one instance of this class.
    */
    private static GameControl instance = null;
    private static readonly object padlock = new object();
    static private AudioSource audioSource;

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


    //easy naming of the screens to prevent having to use magic numbers, if the numbers in Unity change this has to change aswell
    private enum ScenesEnum
    {
        StartScreen = 0,
        MenuScreen = 1,
        TutorialScreen = 2,
        BattleScreen = 3,
        Player1Lost = 4,
        Player2Lost = 5,
        EndScreen = 6,
        CreditScreen = 7,
    }

    //static to make sure the values are not reset after scene schange
    static private int gameRound = 0;
    static private int blueMatchWins = 0;
    static private int redMatchWins = 0;
    static private int gameWinsBlue = 0;
    static private int gameWinsRed = 0;
    // change does not work, TODO still needs fixing // test if works
    [SerializeField] int amountOfMatches = 3;

    public int RetriveGamesWonRed()
    {
        return redMatchWins;
    }
    public int RetriveGamesWonBlue()
    {
        return blueMatchWins;
    }


    void Start()
    {
        DontDestroyOnLoad(this);
        //battleTheme = gameObject.AddComponent<AudioSource>();
        //mainTheme = gameObject.AddComponent<AudioSource>();
        //audioSource = GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        // On load all non-static variables will be reset!

        print(audioSource.clip);
        audioSource.Play();
        

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
        if (blueMatchWins == amountOfMatches)
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
        if (redMatchWins == amountOfMatches)
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
        SoundManager(false);
    }
    public void ResetWins()
    {
        redMatchWins = 0;
        blueMatchWins = 0;
    }


    public void NewRound()
    {        
        SceneManager.LoadScene((int)(ScenesEnum.BattleScreen));
        SoundManager(true);
    }

    public void ChangeToBattleScreen()
    {
        SceneManager.LoadScene((int)(ScenesEnum.BattleScreen));
        SoundManager(true);
    } 

    public void ChangeToMenuScreen()
    {
        SceneManager.LoadScene((int)(ScenesEnum.MenuScreen));
        SoundManager(false);
    } 

    public void ChangeToTutorialScreen()
    {
        SceneManager.LoadScene((int)(ScenesEnum.TutorialScreen));
        SoundManager(false);
    }

    public void ChangeToStartScreen()
    {
        SceneManager.LoadScene((int)(ScenesEnum.StartScreen));
        SoundManager(false);
    }

    public void ChangeToCreditsScreen()
    {
        
        SceneManager.LoadScene((int)(ScenesEnum.CreditScreen));
        SoundManager(false);
    }

    public void SoundManager(bool activeTheme)
    {        
        if (activeTheme)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }
}
