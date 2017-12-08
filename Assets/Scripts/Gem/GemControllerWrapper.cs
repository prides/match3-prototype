using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3Core;

[RequireComponent(typeof(MovingComponent), typeof(Animator))]
public class GemControllerWrapper : GemComponent
{
    #region components
    [SerializeField]
    private GemComponent[] gemComponents;

    [SerializeField]
    private Animator gemAnimator;

    [SerializeField]
    private MovingComponent movingComponent;
    #endregion

    private GemController instance;

    [SerializeField]
    [ReadOnly]
    private GemType type;

    [SerializeField]
    [ReadOnly]
    private GemSpecialType specialType;

    [SerializeField]
    [ReadOnly]
    private int currentX = 0;
    public int CurrentX
    {
        get { return currentX; }
        private set { currentX = value; }
    }

    [SerializeField]
    [ReadOnly]
    private int currentY = 0;
    public int CurrentY
    {
        get { return currentY; }
        private set { currentY = value; }
    }

    public Direction Constrains
    {
        get
        {
            return (instance.LeftNeighbor == null ? Direction.Left : Direction.None)
                | (instance.RightNeighbor == null ? Direction.Right : Direction.None)
                | (instance.UpNeighbor == null ? Direction.Up : Direction.None)
                | (instance.DownNeighbor == null ? Direction.Down : Direction.None);
        }
    }

    public bool IsActive
    {
        get { return instance.IsActive; }
    }

    private void Start()
    {
        movingComponent.OnMovingStartEvent += OnMovingStart;
        movingComponent.OnMovingEndEvent += OnMovingEnd;
    }

    private void OnDestroy()
    {
        this.instance.OnTypeChanged -= OnTypeChanged;
        this.instance.OnPositionChanged -= OnPositionChanged;
        this.instance.OnAppear -= OnAppear;
        this.instance.OnFadeout -= OnFadeout;
        this.instance.OnSpecialTypeChanged -= OnSpecialTypeChanged;

        movingComponent.OnMovingStartEvent -= OnMovingStart;
        movingComponent.OnMovingEndEvent -= OnMovingEnd;
    }

    private void Update()
    {
        if (null == instance)
        {
            return;
        }
        instance.Update();
    }

    public void SetInstance(GemController instance)
    {
        this.instance = instance;
        this.instance.tag = this;
        this.instance.OnTypeChanged += OnTypeChanged;
        this.instance.OnPositionChanged += OnPositionChanged;
        this.instance.OnAppear += OnAppear;
        this.instance.OnFadeout += OnFadeout;
        this.instance.OnSpecialTypeChanged += OnSpecialTypeChanged;
    }

    private void OnAppear(GemController sender, bool animated)
    {
        gameObject.SetActive(true);
        if (animated)
        {
            StartCoroutine(WaitAndTrigger(Randomizer.Range(0.0f, 0.75f), "appear"));
        }
        else
        {
            gemAnimator.SetTrigger("appear");
        }
    }

    private void OnFadeout(GemController sender)
    {
        if (gameObject.activeSelf)
        {
            gemAnimator.SetTrigger("fadeout");
        }
        else
        {
            Debug.Log("activeSelf is false");
        }
    }

    private void OnPositionChanged(GemController sender, int x, int y, bool interpolate)
    {
        CurrentX = x;
        CurrentY = y;
        foreach (GemComponent gemComponent in gemComponents)
        {
            gemComponent.SetPosition(new Vector3(x, y), interpolate);
        }
    }

    private void OnTypeChanged(GemController sender, GemType type)
    {
        this.type = type;
        foreach (GemComponent gemComponent in gemComponents)
        {
            gemComponent.SetGemType(type);
        }
    }

    private void OnSpecialTypeChanged(GemController sender, GemSpecialType specialType)
    {
        this.specialType = specialType;
        foreach (GemComponent gemComponent in gemComponents)
        {
            gemComponent.SetGemSpecialType(specialType);
        }
    }

    private IEnumerator WaitAndTrigger(float wait, string triggerName)
    {
        yield return new WaitForSeconds(wait);
        gemAnimator.SetTrigger(triggerName);
    }

    public void OnSwipeStart(SwipeComponent sender, Direction direction)
    {
        instance.OnMovingStart();
    }

    public void OnSwipeEnd(SwipeComponent sender, Direction direction)
    {
        //Debug.Log("Moving gem(" + currentX + ", " + currentY + ") to " + direction);
        MoveTo(direction);
    }

    public void MoveTo(Direction direction)
    {
        instance.MoveTo(direction);
    }

    private void OnMovingStart(MovingComponent sender)
    {
        //Debug.Log("OnMovingStart " + instance.ToString());
        instance.OnMovingStart();
    }

    private void OnMovingEnd(MovingComponent sender)
    {
        //Debug.Log("OnMovingEnd " + instance.ToString());
        instance.OnMovingEnd();
    }

    private void OnAppearOver()
    {
        instance.OnAppearOver();
    }

    private void OnFadeoutOver()
    {
        instance.OnFadeoutOver();
        gameObject.SetActive(false);
    }
}