using UnityEngine;

public class DetectGameOver : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int loser = collision.gameObject.GetComponent<PlayerControl>().playerID;
        GameControl.Instance.Lose(loser);
    }
}
