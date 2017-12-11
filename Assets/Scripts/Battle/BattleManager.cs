using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public delegate void SimpleCallbackDelegate();

    public enum AttackType
    {
        Close,
        Distance,
        Mass,
        Heal,
        Defence
    }

    [SerializeField]
    private bool is1PlayersTurn = false;

    public BattleUnit player1;
    public BattleUnit player2;

    private void Start()
    {
        player1.OnHealthOverEvent += OnHealthOverEvent;
        player2.OnHealthOverEvent += OnHealthOverEvent;
    }

    private void OnDestroy()
    {
        player1.OnHealthOverEvent -= OnHealthOverEvent;
        player2.OnHealthOverEvent -= OnHealthOverEvent;
    }

    private void OnHealthOverEvent(BattleUnit sender)
    {
        if (player1 == sender)
        {
            Debug.Log("GAME OVER. PLAYER1 LOSE");
        }
        else
        {
            Debug.Log("GAME OVER. PLAYER2 LOSE");
        }
    }

    public void Attack(bool isPlayerAttack, AttackType type, float attackStrength, SimpleCallbackDelegate attackOverCallback)
    {
        switch(type)
        {
            case AttackType.Heal:
            case AttackType.Defence:
                (isPlayerAttack ? player1 : player2).Heal(attackStrength, () => { attackOverCallback(); });
                break;
            case AttackType.Close:
            case AttackType.Distance:
            case AttackType.Mass:
                (isPlayerAttack ? player2 : player1).TakeDamage(type, attackStrength, () => { attackOverCallback(); });
                break;
        }
    }

    public void NextTurn()
    {
        is1PlayersTurn = !is1PlayersTurn;
        player1.SetActive(is1PlayersTurn);
        player2.SetActive(!is1PlayersTurn);
    }
}