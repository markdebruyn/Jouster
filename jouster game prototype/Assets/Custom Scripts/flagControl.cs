using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagControl : MonoBehaviour
{
    public GameObject[] Rounds;
    public GameObject[] redflags;
    public GameObject[] blueflags;
    // Use this for initialization
    void Start ()
    {

        int blueWin = GameControl.Instance.RetriveGamesWonBlue();
        int redWin = GameControl.Instance.RetriveGamesWonRed();

        for (int i = 0; i < blueWin; i++)
        {
            redflags[i].SetActive(true);
        }

        for (int i = 0; i < redWin; i++)
        {
            blueflags[i].SetActive(true);
        }
        for (int i = 0; i < (blueWin + redWin); i++)
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
}
