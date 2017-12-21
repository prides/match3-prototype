using System;
using UnityEngine;

namespace Battle
{
    namespace Unit
    {
        public class SquadUnitReceiver : MonoBehaviour, IHealable, IProtectable, IDamageable
        {
            [SerializeField]
            private SquadUnitController controller;
            [SerializeField]
            private Animator animator;

            public void ReceiveDamage(float damage)
            {
                if (controller.Statistic != null)
                {
                    controller.Statistic.Health -= damage;
                    animator.SetTrigger("Damaged");
                }
            }

            public void ReceiveHeal(float heal)
            {
                if (controller.Statistic != null)
                {
                    controller.Statistic.Health += heal;
                    animator.SetTrigger("Healed");
                }
            }

            public void ReceiveProtection(float protection)
            {
                if (controller.Statistic != null)
                {
                    controller.Statistic.Protection += protection;
                    animator.SetTrigger("Protected");
                }
            }
        }
    }
}