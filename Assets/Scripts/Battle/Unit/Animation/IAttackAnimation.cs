using UnityEngine;

namespace Battle
{
    namespace Unit
    {
        namespace Animation
        {
            internal interface IAttackAnimation
            {
                void DoAnimation(Vector3 target, SimpleCallbackDelegate OnDealDamage, SimpleCallbackDelegate OnAnimationOver);
            }
        }
    }
}