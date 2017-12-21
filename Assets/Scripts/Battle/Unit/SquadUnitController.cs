using UnityEngine;
using Utils;

namespace Battle
{
    namespace Unit
    {
        public class SquadUnitController : MonoBehaviour
        {
            public delegate void SimpleEventDelegate(SquadUnitController sender);
            public event SimpleEventDelegate OnDieEvent;
            public event SimpleEventDelegate OnReviveEvent;

            private UnitType type;
            public UnitType Type
            {
                get { return type; }
                set { type = value; }
            }

            private IDamageable damageable;
            internal IDamageable Damagable
            {
                get
                {
                    if (damageable == null)
                    {
                        damageable = GetComponent<IDamageable>();
                    }
                    return damageable;
                }
            }
            private IHealable healable;
            internal IHealable Healable
            {
                get
                {
                    if (healable == null)
                    {
                        healable = GetComponent<IHealable>();
                    }
                    return healable;
                }
            }
            private IProtectable protectable;
            internal IProtectable Protectable
            {
                get
                {
                    if (protectable == null)
                    {
                        protectable = GetComponent<IProtectable>();
                    }
                    return protectable;
                }
            }
            private IStatistic statistic;
            internal IStatistic Statistic
            {
                get
                {
                    if (statistic == null)
                    {
                        statistic = GetComponent<IStatistic>();
                    }
                    return statistic;
                }
            }
            private IActionable actionable;
            internal IActionable Actionable
            {
                get
                {
                    if (actionable == null)
                    {
                        actionable = GetComponent<IActionable>();
                    }
                    return actionable;
                }
            }

            private void Start()
            {
                Statistic.OnHealthOverEvent += OnDie;
                Statistic.OnReviveEvent += OnRevive;
            }

            private void OnDestroy()
            {
                Statistic.OnHealthOverEvent -= OnDie;
                Statistic.OnReviveEvent -= OnRevive;
            }

            private void OnDie()
            {
                if (OnDieEvent != null)
                {
                    OnDieEvent(this);
                }
            }
            private void OnRevive()
            {
                if (OnReviveEvent != null)
                {
                    OnReviveEvent(this);
                }
            }
        }
    }
}
