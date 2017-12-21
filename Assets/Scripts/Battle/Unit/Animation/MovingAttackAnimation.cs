using UnityEngine;

namespace Battle
{
    namespace Unit
    {
        namespace Animation
        {
            [RequireComponent(typeof(Animator))]
            internal class MovingAttackAnimation : MonoBehaviour, IAttackAnimation
            {
                [SerializeField]
                private MovingComponentBase movingComponent;
                [SerializeField]
                private Animator animator;
                [SerializeField]
                private Vector3 attackPositionOffset;

                private bool isRunning = false;
                private Vector3 startPosition;
                private SimpleCallbackDelegate onOver;

                private void Start()
                {
                    movingComponent.Duration = 1.0f;
                }

                public void DoAnimation(Vector3 target, SimpleCallbackDelegate OnDealDamage, SimpleCallbackDelegate OnAnimationOver)
                {
                    startPosition = transform.position;
                    isRunning = true;
                    onOver = OnAnimationOver;
                    movingComponent.MoveTo(target + attackPositionOffset, true, () =>
                    {
                        OnDealDamage();
                        animator.SetTrigger("Attack");
                    });
                }

                private void OnAttackOver()
                {
                    movingComponent.MoveTo(startPosition, true, () =>
                    {
                        isRunning = false;
                        onOver();
                        onOver = null;
                    });
                }
            }
        }
    }
}