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

    public void Attack(bool isPlayerAttack, AttackType type, float attackStrength, SimpleCallbackDelegate attackOverCallback)
    {
        StartCoroutine(WaitAndAttack(isPlayerAttack, type, attackStrength, attackOverCallback));
    }

    private IEnumerator WaitAndAttack(bool isPlayerAttack, AttackType type, float attackStrength, SimpleCallbackDelegate attackOverCallback)
    {
        yield return new WaitForSeconds(1.0f);
        if (isPlayerAttack)
        {
            Debug.Log("Enemy received " + type + " type " + attackStrength + " damage");
        }
        else
        {
            Debug.Log("Player received " + type + " type " + attackStrength + " damage");
        }
        attackOverCallback();
    }
}