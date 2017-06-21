using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagControl : MonoBehaviour
{
    public AudioSource battleTheme;
    public GameObject[] Rounds;
    public GameObject[] redflags;
    public GameObject[] blueflags;
    // Use this for initialization
    void Start ()
    {        
        int blueWin = GameControl.Instance.RetriveGamesWonBlue();
        int redWin = GameControl.Instance.RetriveGamesWonRed();
        SetBlueFlags(redWin);
        SetRedFlags(blueWin);
        for (int i = 0; i < (Rounds.Length); i++)
        {
            if ((blueWin + redWin) == i)
            {
                Rounds[i].SetActive(true);
            }
            else
            {
                Rounds[i].SetActive(false);
            }
        }
	}

    private void SetBlueFlags(int redWin)
    {
                for (int i = 0; i < redWin; i++)
        {
            blueflags[i].SetActive(true);
        }
    }
    private void SetRedFlags(int blueWin)
    {
        for (int i = 0; i < blueWin; i++)
        {
            redflags[i].SetActive(true);
        }
    }
}
