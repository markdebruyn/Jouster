using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowControl : MonoBehaviour {

    enum ScenesEnum
    {
        StartScreen = 0,
        BattleScreen = 1,
        Player1lost = 2,
        Player2Lost = 3,
        EndScreen = 4,
    }

    void SwitchGameScene(int levelCode)
    {
        SceneManager.LoadScene(levelCode);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
