using UnityEngine;
using System.Collections;
using Match3Core;

using UI;
using AI;
using Battle;
using Match3Wrapper;
using ActionQueue;
using PlayerInput;

public delegate void SimpleCallbackDelegate();

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GemManagerWrapper gemManager;
    [SerializeField]
    private BattleActionsQueue battleActionsQueue;
    [SerializeField]
    private BattleManagerBase battleManager;
    [SerializeField]
    private AIController aiController;
    [SerializeField]
    private SwipeComponent swipeComponent;
    [SerializeField]
    private TextValueChanger moneyValueChanger;
    [SerializeField]
    private bool isPlayerMove = false;
    [SerializeField]
    [ReadOnly]
    private int currentMoney = 0;
    public int CurrentMoney
    {
        get { return currentMoney; }
        set
        {
            currentMoney = value >= 0 ? value : 0;
            if (moneyValueChanger != null)
            {
                moneyValueChanger.OnValueChange(this, currentMoney);
            }
        }
    }

    private void Start()
    {
        swipeComponent.SetActive(true);
        gemManager.OnGemPointsReceivedEvent += OnGemPointsReceivedEvent;
        gemManager.OnReadyChangedEvent += OnMatchReadyChangedEvent;
        battleManager.OnPlayerLoseEvent += OnPlayerLose;
        battleManager.Init();
    }

    private void OnDestroy()
    {
        battleManager.OnPlayerLoseEvent -= OnPlayerLose;
        battleManager.Deinit();
        gemManager.OnGemPointsReceivedEvent -= OnGemPointsReceivedEvent;
        gemManager.OnReadyChangedEvent -= OnMatchReadyChangedEvent;
    }

    private bool isGameOver;
    [SerializeField]
    private GameOverTextController gameoverText;
    private void OnPlayerLose(BattleManagerBase sender, int playerNum)
    {
        swipeComponent.SetActive(false);
        if (playerNum == 1)
        {
            gameoverText.ShowLoseText();
        }
        else
        {
            gameoverText.ShowWinText();
        }
        isGameOver = true;
    }

    private void OnMatchReadyChangedEvent(object sender, bool ready)
    {
        if (isGameOver)
        {
            return;
        }
        if (ready)
        {
            if (isPlayerMove && battleActionsQueue.GetQueueLength() == 0)
            {
                swipeComponent.SetActive(true);
            }
            else
            {
                TryToAttack();
            }
        }
        else
        {
            swipeComponent.SetActive(false);
        }
    }

    private void OnGemPointsReceivedEvent(object sender, GemPoint[] gemPoints)
    {
        battleActionsQueue.AddGemPoints(gemPoints);
    }

    private void TryToAttack()
    {
        GemPoint[] gemPoints = battleActionsQueue.GetGemPoints();
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
        ActionType attackType = GetAttackTypeByGemType(gemtype);
        battleManager.Action(attackType, attackStrength, () => { StartCoroutine(WaitAndTryToAttack(0.1f)); });
    }

    private IEnumerator WaitAndTryToAttack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        TryToAttack();
    }

    private void GiveMoney(float strength)
    {
        if (isPlayerMove)
        {
            CurrentMoney += (int)strength;
            Debug.Log("received " + (int)strength + " money");
        }
    }

    private void NextMove()
    {
        battleManager.NextTurn();
        isPlayerMove = !isPlayerMove;
        if (isPlayerMove)
        {
            swipeComponent.SetActive(true);
        }
        else
        {
            AiMove();
        }
    }

    private void AiMove()
    {
        aiController.Move(gemManager.PossibleMoves.ToArray());
    }

    private ActionType GetAttackTypeByGemType(GemType type)
    {
        switch(type)
        {
            case GemType.Close:
                return ActionType.Close;
            case GemType.Defence:
                return ActionType.Defence;
            case GemType.Distance:
                return ActionType.Distance;
            case GemType.Heal:
                return ActionType.Heal;
            case GemType.Mass:
                return ActionType.Mass;
            default:
                Debug.LogError("unknown gem type: " + type);
                return ActionType.Close;
        }
    }
}