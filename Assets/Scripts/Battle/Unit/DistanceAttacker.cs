using UnityEngine;
using Utils;

namespace Battle
{
    namespace Unit
    {
        internal class DistanceAttacker : Attacker
        {
            public override void DoAction(float strength, SquadContainer teammates, SquadContainer enemies, SimpleCallbackDelegate onOverCallback)
            {
                if (enemies == null)
                {
                    Utils.Logger.GetInstance().Error("enemies is null");
                    onOverCallback();
                    return;
                }
                SquadUnitController[] distancelineEnemies = enemies.GetDistanceLineUnits();
                if (distancelineEnemies == null || distancelineEnemies.Length <= 0)
                {
                    Utils.Logger.GetInstance().Message("failed to get distanceline enemies");
                    onOverCallback();
                    return;
                }
                SquadUnitController enemy = distancelineEnemies[Randomizer.Range(0, distancelineEnemies.Length)];
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