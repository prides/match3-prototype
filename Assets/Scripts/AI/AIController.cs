using UnityEngine;
using System.Collections;

using Match3Wrapper;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        public void Move(PossibleMoveWrapper[] possibleMoves)
        {
            StartCoroutine(WaitAndMove(Random.Range(0.1f, 1.0f), possibleMoves[Random.Range(0, possibleMoves.Length)]));
        }

        private IEnumerator WaitAndMove(float waitTime, PossibleMoveWrapper move)
        {
            yield return new WaitForSeconds(waitTime);
            move.Move();
        }
    }
}