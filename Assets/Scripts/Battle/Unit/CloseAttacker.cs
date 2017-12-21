using UnityEngine;
using Battle.Unit.Animation;
using Utils;

namespace Battle
{
    namespace Unit
    {
        internal class CloseAttacker : Attacker
        {
            public override void DoAction(float strength, SquadContainer teammates, SquadContainer enemies, SimpleCallbackDelegate onOverCallback)
            {
                if (enemies == null)
                {
                    Utils.Logger.GetInstance().Error("enemies is null");
                    onOverCallback();
                    return;
                }
                SquadUnitController[] frontlineEnemies = enemies.GetFrontLineUnits();
                if (frontlineEnemies == null || frontlineEnemies.Length <= 0)
                {
                    Utils.Logger.GetInstance().Message("failed to get frontline enemies");
                    onOverCallback();
                    return;
                }
                Utils.Logger.GetInstance().Message("frontlineEnemies.Length: " + frontlineEnemies.Length);
                SquadUnitController enemy = frontlineEnemies[Randomizer.Range(0, frontlineEnemies.Length)];
                if (enemy == null || enemy.Damagable == null)
                {
                    onOverCallback();
                    return;
                }
                AttackAnimation.DoAnimation(enemy.transform.position, () =>
                {
                    enemy.Damagable.ReceiveDamage(strength);
                }, () =>
                {
                    onOverCallback();
                });
            }
        }
    }
}