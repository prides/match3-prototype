using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class SquadBattleManager : BattleManagerBase
    {
        [SerializeField]
        private SquadController leftSquad;
        [SerializeField]
        private SquadController rightSquad;

        private bool isLeftsTurn = false;

        public override void Init()
        {
            leftSquad.Init();
            rightSquad.Init();

            leftSquad.EnemySquad = rightSquad.CurrentSquad;
            rightSquad.EnemySquad = leftSquad.CurrentSquad;

            leftSquad.OnSquadDefeatedEvent += OnSquadDefeatedEvent;
            rightSquad.OnSquadDefeatedEvent += OnSquadDefeatedEvent;
        }

        public override void Deinit()
        {
            leftSquad.OnSquadDefeatedEvent -= OnSquadDefeatedEvent;
            rightSquad.OnSquadDefeatedEvent -= OnSquadDefeatedEvent;
        }

        private void OnSquadDefeatedEvent(SquadController sender)
        {
            if (sender == leftSquad)
            {
                OnPlayerLose(this, 1);
            }
            else
            {
                OnPlayerLose(this, 2);
            }
        }

        public override void Action(ActionType type, float attackStrength, SimpleCallbackDelegate attackOverCallback)
        {
            (isLeftsTurn ? leftSquad : rightSquad).Action(type, attackStrength, attackOverCallback);
        }

        public override void NextTurn()
        {
            isLeftsTurn = !isLeftsTurn;
            leftSquad.SetActive(isLeftsTurn);
            rightSquad.SetActive(!isLeftsTurn);
        }
    }
}
