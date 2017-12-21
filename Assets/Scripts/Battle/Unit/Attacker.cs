using UnityEngine;
using Battle.Unit.Animation;
using Utils;

namespace Battle
{
    namespace Unit
    {
        internal abstract class Attacker : MonoBehaviour, IActionable
        {
            private IAttackAnimation attackAnimation;
            internal IAttackAnimation AttackAnimation
            {
                get
                {
                    if (attackAnimation == null)
                    {
                        attackAnimation = GetComponent<IAttackAnimation>();
                    }
                    return attackAnimation;
                }
            }

            public abstract void DoAction(float strength, SquadContainer teammates, SquadContainer enemies, SimpleCallbackDelegate onOverCallback);
        }
    }
}