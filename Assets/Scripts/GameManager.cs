using UnityEngine;
using System.Collections;
using Match3Core;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GemManagerWrapper gemManager;
    [SerializeField]
    private BrokenGemsQueue brokenGemsQueue;
    [SerializeField]
    private BattleManager battleManager;
    [SerializeField]
    private AIController aiController;
    [SerializeField]
    private SwipeComponent swipeComponent;
    [SerializeField]
    private bool isPlayerMove = false;

    private void Start()
    {
        swipeComponent.SetActive(true);
        gemManager.OnGemPointsReceivedEvent += OnGemPointsReceivedEvent;
        gemManager.OnReadyChangedEvent += OnMatchReadyChangedEvent;
    }

    private void OnMatchReadyChangedEvent(object sender, bool value)
    {
        if (value)
        {
            TryToAttack();
        }
        else
        {
            swipeComponent.SetActive(false);
        }
    }

    private void OnGemPointsReceivedEvent(object sender, GemPoint[] gemPoints)
    {
        brokenGemsQueue.AddGemPoints(gemPoints);
    }

    private void TryToAttack()
    {
        GemPoint[] gemPoints = brokenGemsQueue.GetGemPoints();
        if (gemPoints == null)
        {
            NextMove();
            return;
        }
        GemType gemtype = GemType.None;
        float attackStrength = 0.0f;
        foreach (GemPoint gp in gemPoints)
        {
            if (gemtype == GemType.None)
            {
                gemtype = gp.type;
            }
            attackStrength += gp.weight;
        }
        if (gemtype == GemType.Money)
        {
            GiveMoney(attackStrength);
            StartCoroutine(WaitAndTryToAttack(1.0f));
            return;
        }
        BattleManager.AttackType attackType = GetAttackTypeByGemType(gemtype);
        battleManager.Attack(isPlayerMove, attackType, attackStrength, () => { TryToAttack(); });
    }

    private IEnumerator WaitAndTryToAttack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        TryToAttack();
    }

    private void GiveMoney(float strength)
    {
        Debug.Log("received " + (int)strength + " money");
    }

    private void NextMove()
    {
        battleManager.NextTurn();
        if (isPlayerMove)
        {
            isPlayerMove = !isPlayerMove;
            AiMove();
        }
        else
        {
            isPlayerMove = !isPlayerMove;
            swipeComponent.SetActive(true);
        }
    }

    private void AiMove()
    {
        aiController.Move(gemManager.PossibleMoves.ToArray());
    }

    private BattleManager.AttackType GetAttackTypeByGemType(GemType type)
    {
        switch(type)
        {
            case GemType.Close:
                return BattleManager.AttackType.Close;
            case GemType.Defence:
                return BattleManager.AttackType.Defence;
            case GemType.Distance:
                return BattleManager.AttackType.Distance;
            case GemType.Heal:
                return BattleManager.AttackType.Heal;
            case GemType.Mass:
                return BattleManager.AttackType.Mass;
            default:
                Debug.LogError("unknown gem type: " + type);
                return BattleManager.AttackType.Close;
        }
    }
}