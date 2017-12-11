using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class BattleUnit : MonoBehaviour
{
    public delegate void SimpleCallbackDelegate();

    public delegate void SimpleEventDelegate(BattleUnit sender);
    public event SimpleEventDelegate OnHealthOverEvent;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Vector3 textOffset;

    [SerializeField]
    private ProgressBar healthBar;
    [SerializeField]
    private TextMesh healthText;

    [SerializeField]
    [ReadOnly]
    private float health = 100.0f;
    public float Health
    {
        get { return health; }
        private set
        {
            value = Mathf.Clamp(value, 0.0f, 100.0f);
            float diff = value - health;
            TextSpawner.GetInstance().SpawnTextMessage(diff > 0 ? "+" + diff : diff.ToString(), transform.position + textOffset, diff < 0 ? Color.red : Color.green);
            health = value;
            healthBar.SetProgress(health / 100.0f);
            healthText.text = ((int)health) + "/100";
        }
    }

    public void SetActive(bool value)
    {
        animator.SetBool("isActive", value);
        animator.SetTrigger("activeStateChanged");
    }

    private SimpleCallbackDelegate takeDamageCallback;
    public void TakeDamage(BattleManager.AttackType type, float attackStrength, SimpleCallbackDelegate attackOverCallback)
    {
        takeDamageCallback = attackOverCallback;
        animator.SetTrigger("takeDamage");
        Health -= attackStrength;
        if (Health <= 0.0f)
        {
            OnHealthOverEvent(this);
        }
    }

    public void Heal(float strength, SimpleCallbackDelegate attackOverCallback)
    {
        Health += strength;
        StartCoroutine(WaitAndCall(1.0f, attackOverCallback));
    }

    private IEnumerator WaitAndCall(float waitTime, SimpleCallbackDelegate callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback();
    }

    public void OnTakeDamageOver()
    {
        if (takeDamageCallback != null)
        {
            takeDamageCallback();
        }
    }
}
