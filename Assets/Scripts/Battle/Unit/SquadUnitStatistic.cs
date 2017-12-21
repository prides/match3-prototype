using UnityEngine;
using UI;
using System;

namespace Battle
{
    namespace Unit
    {
        internal class SquadUnitStatistic : MonoBehaviour, IStatistic
        {
            [SerializeField]
            private ProgressBar healthBar;
            [SerializeField]
            private TextMesh healthText;

            [SerializeField]
            private Vector3 textOffset;

            [SerializeField]
            [ReadOnly]
            private bool isDied = false;
            public bool IsDied
            {
                get { return isDied; }
            }

            [SerializeField]
            private RangeAttribute healthOverRange = new RangeAttribute(-20.0f, 0.0f);
            [SerializeField]
            private RangeAttribute healthRange = new RangeAttribute(0.0f, 30.0f);
            [SerializeField]
            private float health = 30.0f;
            public float Health
            {
                get { return health; }
                set
                {
                    if (isDied)
                    {
                        value = Mathf.Clamp(value, healthOverRange.min, healthOverRange.max);
                    }
                    else
                    {
                        value = Mathf.Clamp(value, healthRange.min, healthRange.max);
                    }
                    float diff = value - health;
                    if (isDied)
                    {
                        if (value >= 0.0f)
                        {
                            isDied = false;
                            health = healthRange.max;
                            OnReviveEvent();
                            return;
                        }
                        if (diff <= 0.0f)
                        {
                            return;
                        }
                    }
                    if (!isDied && diff < 0.0f && Protection > 0.0f)
                    {
                        float protDamage = Protection > -diff ? diff : -Protection;
                        diff -= protDamage;
                        Protection += protDamage;
                        TextSpawner.GetInstance().SpawnTextMessage(((int)protDamage).ToString(), transform.position + textOffset, Color.blue);
                        if (diff <= float.Epsilon)
                        {
                            return;
                        }
                    }
                    TextSpawner.GetInstance().SpawnTextMessage(diff > 0 ? "+" + diff : diff.ToString(), transform.position + textOffset, diff < 0 ? Color.red : Color.green);
                    health = value;
                    RefreshHealthUI();
                    if (!isDied && health <= 0.0f)
                    {
                        isDied = true;
                        health = healthOverRange.min;
                        RefreshHealthUI();
                        OnHealthOverEvent();
                    }
                }
            }

            private void Start()
            {
                RefreshHealthUI();
            }

            private void RefreshHealthUI()
            {
                healthBar.SetProgress(health / healthRange.max);
                healthText.text = ((int)health) + "/" + (int)healthRange.max;
            }

            [SerializeField]
            private RangeAttribute protectionRange = new RangeAttribute(0.0f, 999.0f);
            [SerializeField]
            private float protection = 0.0f;
            public float Protection
            {
                get { return protection; }
                set
                {
                    value = Mathf.Clamp(value, protectionRange.min, protectionRange.max);
                    if (!isDied)
                    {
                        protection = value;
                    }
                }
            }

            private SimpleCallbackDelegate onHealthOverEvent;
            public SimpleCallbackDelegate OnHealthOverEvent
            {
                get { return onHealthOverEvent; }
                set { onHealthOverEvent = value; }
            }

            private SimpleCallbackDelegate onReviveEvent;
            public SimpleCallbackDelegate OnReviveEvent
            {
                get { return onReviveEvent; }
                set { onReviveEvent = value; }
            }
        }
    }
}