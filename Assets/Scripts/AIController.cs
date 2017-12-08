using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{
    public void Move(GemManagerWrapper.PossibleMoveWrapper[] possibleMoves)
    {
        StartCoroutine(WaitAndMove(Random.Range(0.4f, 2.0f), possibleMoves[Random.Range(0, possibleMoves.Length)]));
    }

    private IEnumerator WaitAndMove(float waitTime, GemManagerWrapper.PossibleMoveWrapper move)
    {
        yield return new WaitForSeconds(waitTime);
        move.Move();
    }
}