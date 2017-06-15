using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenInfoScript : MonoBehaviour {

    public GameObject redWin;
    public GameObject blueWin;
    // Use this for initialization
    void Start ()
    {
        int blueWins = GameControl.Instance.RetriveGamesWonBlue();
        int redWins = GameControl.Instance.RetriveGamesWonRed();
        redWin.SetActive(blueWins > redWins);
        blueWin.SetActive(blueWins < redWins);
    }

}
