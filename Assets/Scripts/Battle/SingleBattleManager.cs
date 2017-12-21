using System;
using UnityEngine;

namespace Battle
{
    public class SingleBattleManager : BattleManagerBase
    {
        [SerializeField]
        private bool is1PlayersTurn = false;

        public SingleBattleUnit player1;
        public SingleBattleUnit player2;

        public override void Init()
        {
            player1.OnHealthOverEvent += OnHealthOverEvent;
            player2.OnHealthOverEvent += OnHealthOverEvent;
        }

        public override void Deinit()
        {
            player1.OnHealthOverEvent -= OnHealthOverEvent;
            player2.OnHealthOverEvent -= OnHealthOverEvent;
        }

        private void OnHealthOverEvent(SingleBattleUnit sender)
        {
            OnPlayerLose(this, player1 == sender ? 1 : 2);
            Debug.Log("GAME OVER. PLAYER" + (player1 == sender ? "1" : "2") + " LOSE");
        }

        public override void Action(ActionType type, float attackStrength, SimpleCallbackDelegate attackOverCallback)
        {
            switch (type)
            {
                case ActionType.Heal:
                case ActionType.Defence:
                    (is1PlayersTurn ? player1 : player2).Heal(attackStrength, () => { attackOverCallback(); });
                    break;
                case ActionType.Close:
                case ActionType.Distance:
                case ActionType.Mass:
                    (is1PlayersTurn ? player2 : player1).TakeDamage(type, attackStrength, () => { attackOverCallback(); });
                    break;
            }
        }

        public override void NextTurn()
        {
            is1PlayersTurn = !is1PlayersTurn;
            player1.SetActive(is1PlayersTurn);
            player2.SetActive(!is1PlayersTurn);
        }
    }
}