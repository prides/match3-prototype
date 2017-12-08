using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Animator))]
public class BrokenGemController : MonoBehaviour
{
    public delegate void SimpleEventDelegate();

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private MovingComponent movingComponent;
    [SerializeField]
    private GemSpriteSwitcher gemSpriteSwitcher;
    [SerializeField]
    private GemPoint[] gemPoints = null;
    public GemPoint[] GemPoints
    {
        get { return gemPoints; }
        set
        {
            gemPoints = value;
            SetGemCount(gemPoints != null ? gemPoints.Length : 0);
            gemSpriteSwitcher.SetGemType(gemPoints != null && gemPoints.Length > 0 ? gemPoints[0].type : Match3Core.GemType.None);
        }
    }

    public void SetPosition(int index)
    {
        movingComponent.SetPosition(new Vector3(index, 0.0f, 0.0f), true);
    }

    private void SetGemCount(int count)
    {

    }

    private SimpleEventDelegate appearOverCallback;
    public void Appear(SimpleEventDelegate appearOverCallback)
    {
        this.appearOverCallback = appearOverCallback;
        animator.SetTrigger("appear");
    }

    private SimpleEventDelegate dissappearOverCallback;
    public void Disappear(SimpleEventDelegate dissappearOverCallback)
    {
        this.dissappearOverCallback = dissappearOverCallback;
        animator.SetTrigger("fadeout");
    }

    private void OnAppearOver()
    {
        if (null != appearOverCallback)
        {
            appearOverCallback();
            appearOverCallback = null;
        }
    }

    private void OnFadeoutOver()
    {
        if (null != dissappearOverCallback)
        {
            dissappearOverCallback();
            dissappearOverCallback = null;
        }
        Destroy(gameObject);
    }
}