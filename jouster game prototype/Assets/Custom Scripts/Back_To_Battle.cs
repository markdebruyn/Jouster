using UnityEngine;

public class Back_To_Battle : MonoBehaviour
{
    public float waitTimeInSeconds = 5;
    System.Collections.IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(waitTimeInSeconds);
        GameControl.Instance.NewRound();
        yield break;
    }

    public void Start()
    {
        StartCoroutine("StartBattle");
    }
}
