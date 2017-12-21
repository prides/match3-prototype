using UnityEngine;

namespace Battle
{
    namespace Unit
    {
        namespace Animation
        {
            [RequireComponent(typeof(Animator))]
            internal class StandAttackAnimation : MonoBehaviour, IAttackAnimation
            {
                [SerializeField]
                private Animator animator;

                private bool isRunning = false;
                private SimpleCallbackDelegate onDealDamage;
                private SimpleCallbackDelegate onOver;

                public void DoAnimation(Vector3 target, SimpleCallbackDelegate OnDealDamage, SimpleCallbackDelegate OnAnimationOver)
                {
                    isRunning = true;
                    onOver = OnAnimationOver;
                    onDealDamage = OnDealDamage;
                    animator.SetTrigger("Prepare");
                }

                private void OnPreparationOver()
                {
                    onDealDamage();
                    animator.SetTrigger("Attack");
                }

                private void OnAttackOver()
                {
                    onOver();
                }
            }
        }
    }
}