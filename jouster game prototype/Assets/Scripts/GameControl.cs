using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    enum ScenesEnum
    {
        StartScreen = 0,
        BattleScreen= 1,
        Player1lost = 2,
        Player2Lost = 3,
        EndScreen   = 4,
    } 


    // does not work like this
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int loser = collision.gameObject.GetComponent<PlayerControl>().playerID;
        if (loser == 1)
        {
            SceneManager.LoadScene(ScenesEnum.Player1lost.ToString());
        }
        if (loser == 2)
        {
            SceneManager.LoadScene(ScenesEnum.Player2Lost.ToString());
        }
    }
    // Use this for initialization
    void Start ()

    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
