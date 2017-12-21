using System.Collections;
using UnityEngine;

using Battle.Unit;

namespace Battle
{
    public class SquadController : MonoBehaviour
    {
        public delegate void SimpleEventDelegate(SquadController sender);
        internal event SimpleEventDelegate OnSquadDefeatedEvent;

        private SquadContainer currentSquad;
        internal SquadContainer CurrentSquad
        {
            get { return currentSquad; }
        }
        private SquadContainer enemySquad;
        internal SquadContainer EnemySquad
        {
            get { return enemySquad; }
            set { enemySquad = value; }
        }

        private string[] prefabPaths = new string[] { "Prefabs/Units/CloseUnit", "Prefabs/Units/MassUnit", "Prefabs/Units/DistanceUnit", "Prefabs/Units/HealerUnit" };
        internal void Init()
        {
            currentSquad = new SquadContainer(2, 2);
            for (int i = 0; i < 4; i++)
            {
                UnitType type = i == 0 ? UnitType.CloseAttack : i == 1 ? UnitType.MassiveAttack : i == 2 ? UnitType.DistanceAttack : UnitType.Healer;
                GameObject prefab = (GameObject)Resources.Load(prefabPaths[i]);
                GameObject go = Instantiate(prefab);
                go.transform.parent = this.transform;
                if (this.transform.localPosition.x < 0.0f)
                {
                    go.transform.localPosition = new Vector3(i / 2 == 0 ? 1.5f : -1.5f, 1.0f, i % 2 == 0 ? 1.5f : -1.5f);
                }
                else
                {
                    go.transform.localPosition = new Vector3(i / 2 == 0 ? -1.5f : 1.5f, 1.0f, i % 2 == 0 ? 1.5f : -1.5f);
                }
                SquadUnitController unit = (SquadUnitController)go.GetComponent<SquadUnitController>();
                currentSquad.SetUnit(unit, i / 2, i % 2);
                unit.Type = type;
                unit.OnDieEvent += OnUnitDiedEvent;
                unit.OnReviveEvent += OnUnitReviveEvent;
            }
        }

        private int diedUnitsCount = 0;
        private void OnUnitReviveEvent(SquadUnitController sender)
        {
            diedUnitsCount--;
        }

        private void OnUnitDiedEvent(SquadUnitController sender)
        {
            diedUnitsCount++;
            if (diedUnitsCount == 4)
            {
                Utils.Logger.GetInstance().Message(gameObject.name + ": GAME OVER");
                if (null != OnSquadDefeatedEvent)
                {
                    OnSquadDefeatedEvent(this);
                }
            }
        }

        internal void Deinit()
        {
            SquadUnitController[] units = currentSquad.GetAllUnits();
            foreach (SquadUnitController unit in units)
            {
                unit.OnDieEvent -= OnUnitDiedEvent;
                unit.OnReviveEvent -= OnUnitReviveEvent;
            }
        }

        internal void Action(ActionType type, float attackStrength, SimpleCallbackDelegate attackOverCallback)
        {
            if (type == ActionType.Defence)
            {
                Defence(attackStrength, attackOverCallback);
                return;
            }
            SquadUnitController unit = currentSquad.GetUnitByType(GetUnitTypeByActionType(type));
            if (unit == null)
            {
                Utils.Logger.GetInstance().Message("Failed to fine unit with type Close");
                return;
            }
            if (unit.Actionable != null)
            {
                if (unit.Statistic != null && !unit.Statistic.IsDied)
                {
                    unit.Actionable.DoAction(attackStrength, currentSquad, enemySquad, attackOverCallback);
                }
                else
                {
                    attackOverCallback();
                }
            }
        }

        private void Defence(float strength, SimpleCallbackDelegate onOverCallback)
        {
            SquadUnitController[] units = currentSquad.GetAllUnits();
            if (units == null)
            {
                return;
            }
            foreach (SquadUnitController unit in units)
            {
                unit.Protectable.ReceiveProtection(strength);
            }
            StartCoroutine(WaitAndRun(1.0f, onOverCallback));
        }

        private IEnumerator WaitAndRun(float waittime, SimpleCallbackDelegate action)
        {
            yield return new WaitForSeconds(waittime);
            action();
        }

        private UnitType GetUnitTypeByActionType(ActionType type)
        {
            switch (type)
            {
                case ActionType.Close:
                    return UnitType.CloseAttack;
                case ActionType.Distance:
                    return UnitType.DistanceAttack;
                case ActionType.Mass:
                    return UnitType.MassiveAttack;
                case ActionType.Heal:
                    return UnitType.Healer;
                default:
                    return UnitType.Invalid;
            }
        }

        internal void SetActive(bool value)
        {
            if (value)
            {
                SquadUnitController[] units = currentSquad.GetAllUnits();
                if (units == null)
                {
                    return;
                }
                foreach (SquadUnitController unit in units)
                {
                    if (unit.Statistic.Protection > 0.0f)
                    {
                        unit.Statistic.Protection = 0.0f;
                    }
                }
            }
        }
    }
}
