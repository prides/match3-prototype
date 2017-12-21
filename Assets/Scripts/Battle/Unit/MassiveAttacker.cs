using UnityEngine;
using Utils;

namespace Battle
{
    namespace Unit
    {
        internal class MassiveAttacker : Attacker
        {
            public override void DoAction(float strength, SquadContainer teammates, SquadContainer enemies, SimpleCallbackDelegate onOverCallback)
            {
                if (enemies == null)
                {
                    Utils.Logger.GetInstance().Error("enemies is null");
                    onOverCallback();
                    return;
                }
                SquadUnitController[] units = enemies.GetAllUnits();
                if (units == null || units.Length <= 0)
                {
                    Utils.Logger.GetInstance().Error("Failed to get enemy units");
                    onOverCallback();
                    return;
                }
                AttackAnimation.DoAnimation(units[0].transform.parent.position, () =>
                {
                    foreach (SquadUnitController unit in units)
                    {
                        if (unit.Damagable != null)
                        {
                            unit.Damagable.ReceiveDamage(strength);
                        }
                    }
                }, () =>
                {
                    onOverCallback();
                });
            }
        }
    }
}