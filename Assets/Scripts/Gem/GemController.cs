﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwipeComponent), typeof(MovingComponent), typeof(Animator))]
public class GemController : GemComponent
{
    #region events
    public delegate void SimpleGemEventDelegate(GemController sender);
    public event SimpleGemEventDelegate OnReadyEvent;

    public delegate void PossibleMatchDelegate(GemController sender, PossibleMatch possibleMatch);
    public event PossibleMatchDelegate OnPossibleMatchAddedEvent;
    
    public delegate void EventWithDirectionDelegate(GemController sender, Direction direction);
    public event EventWithDirectionDelegate OnMovingToEvent;
    #endregion

    #region components
    [SerializeField]
    private GemComponent[] gemComponents;

    [SerializeField]
    private Animator gemAnimator;

    [SerializeField]
    private MovingComponent movingComponent;

    [SerializeField]
    private SwipeComponent swipeComponent;
    #endregion

    #region properties
    [SerializeField]
    [ReadOnly]
    private GemType currentGemType = 0;
    public GemType CurrentGemType
    {
        get { return currentGemType; }
        private set { currentGemType = value; }
    }

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

    [SerializeField]
    [ReadOnly]
    private bool isActive = false;
    public bool IsActive
    {
        get { return isActive; }
    }

    private List<PossibleMatch> possibleMatches = new List<PossibleMatch>();
    public List<PossibleMatch> PossibleMatches
    {
        get { return possibleMatches; }
    }
    #endregion

    #region neighbor
    [Flags]
    public enum NeighborChanged
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8
    }

    private GemController leftNeighbor = null;
    private GemController rightNeighbor = null;
    private GemController upNeighbor = null;
    private GemController downNeighbor = null;

    private NeighborChanged neighborChangedFlag = NeighborChanged.None;

    public GemController LeftNeighbor
    {
        get { return leftNeighbor; }
        set
        {
            leftNeighbor = value;
            neighborChangedFlag |= NeighborChanged.Left;
            if (leftNeighbor == null)
            {
                swipeComponent.AddConstrain(Direction.Left);
            }
            else
            {
                swipeComponent.RemoveConstrain(Direction.Left);
            }
        }
    }

    public GemController RightNeighbor
    {
        get { return rightNeighbor; }
        set
        {
            rightNeighbor = value;
            neighborChangedFlag |= NeighborChanged.Right;
            if (rightNeighbor == null)
            {
                swipeComponent.AddConstrain(Direction.Right);
            }
            else
            {
                swipeComponent.RemoveConstrain(Direction.Right);
            }
        }
    }

    public GemController UpNeighbor
    {
        get { return upNeighbor; }
        set
        {
            upNeighbor = value;
            neighborChangedFlag |= NeighborChanged.Up;
            if (upNeighbor == null)
            {
                swipeComponent.AddConstrain(Direction.Up);
            }
            else
            {
                swipeComponent.RemoveConstrain(Direction.Up);
            }
        }
    }

    public GemController DownNeighbor
    {
        get { return downNeighbor; }
        set
        {
            downNeighbor = value;
            neighborChangedFlag |= NeighborChanged.Down;
            if (downNeighbor == null)
            {
                swipeComponent.AddConstrain(Direction.Down);
            }
            else
            {
                swipeComponent.RemoveConstrain(Direction.Down);
            }
        }
    }

    private SwipeAction currentSwipeAction = null;
    public SwipeAction CurrentSwipeAction
    {
        get { return currentSwipeAction; }
        set { currentSwipeAction = value; }
    }
    #endregion

    #region unity events
    private void Start()
    {
        movingComponent.OnMovingStartEvent += OnMovingStart;
        movingComponent.OnMovingEndEvent += OnMovingEnd;

        swipeComponent.OnSwipeStartEvent += OnSwipeStart;
        swipeComponent.OnSwipeEndEvent += OnSwipeEnd;
    }

    private void OnDestroy()
    {
        movingComponent.OnMovingStartEvent -= OnMovingStart;
        movingComponent.OnMovingEndEvent -= OnMovingEnd;

        swipeComponent.OnSwipeStartEvent -= OnSwipeStart;
        swipeComponent.OnSwipeEndEvent -= OnSwipeEnd;
    }

    private void Update()
    {
        if (neighborChangedFlag != NeighborChanged.None)
        {
            OnNeighborChanged();
        }
    }
    #endregion

    public enum State
    {
        Idle,
        Moving,
        Matched
    }

    private State currentState;
    public State CurrentState
    {
        get { return currentState; }
    }

    public void Init(bool randomAppear = true)
    {
        isActive = true;
        StartCoroutine(WaitAndTrigger(randomAppear ? Randomizer.Range(0.0f, 0.75f) : 0.0f, "appear"));
    }

    private IEnumerator WaitAndTrigger(float wait, string triggerName)
    {
        yield return new WaitForSeconds(wait);
        gemAnimator.SetTrigger(triggerName);
    }

    public override void SetGemType(GemType type)
    {
        currentGemType = type;
        foreach (GemComponent gemComponent in gemComponents)
        {
            gemComponent.SetGemType(type);
        }
    }

    public override void SetPosition(int x, int y, bool interpolate = false)
    {
        CurrentX = x;
        CurrentY = y;
        foreach (GemComponent gemComponent in gemComponents)
        {
            gemComponent.SetPosition(x, y, interpolate);
        }
    }

    public GemController GetNeighbor(Direction direction)
    {
        switch(direction)
        {
            case Direction.Left:
                return this.LeftNeighbor;
            case Direction.Right:
                return this.RightNeighbor;
            case Direction.Up:
                return this.UpNeighbor;
            case Direction.Down:
                return this.DownNeighbor;
            default:
                return null;
        }
    }

    private void OnNeighborChanged()
    {
        if ((neighborChangedFlag & NeighborChanged.Left) == NeighborChanged.Left)
        {
            CheckNeighbor(leftNeighbor);
        }
        if ((neighborChangedFlag & NeighborChanged.Right) == NeighborChanged.Right)
        {
            CheckNeighbor(rightNeighbor);
        }
        if ((neighborChangedFlag & NeighborChanged.Up) == NeighborChanged.Up)
        {
            CheckNeighbor(upNeighbor);
        }
        if ((neighborChangedFlag & NeighborChanged.Down) == NeighborChanged.Down)
        {
            CheckNeighbor(downNeighbor);
        }

        if (possibleMatches.Count > 1)
        {
            List<PossibleMatch> posMatch = new List<PossibleMatch>(possibleMatches);
            for (int i = 0; i < posMatch.Count; i++)
            {
                for (int j = 0; j < posMatch.Count; j++)
                {
                    if (posMatch[i] == posMatch[j])
                    {
                        continue;
                    }
                    if (posMatch[i].MatchDirection == posMatch[j].MatchDirection)
                    {
                        posMatch[i].Merge(posMatch[j]);
                    }
                    else if (posMatch[i].IsMatched() && posMatch[j].IsMatched())
                    {
                        posMatch[i].Merge(posMatch[j]);
                    }
                }
            }
        }

        bool isMatched = false;
        for (int i = possibleMatches.Count - 1; i >= 0; i--)
        {
            if (possibleMatches[i].CheckMatch())
            {
                isMatched = true;
            }
        }

        if (currentState == State.Idle && null != currentSwipeAction)
        {
            currentSwipeAction.SetGemSwipeResult(this, isMatched);
        }

        neighborChangedFlag = NeighborChanged.None;
    }

    private void CheckNeighbor(GemController neighbor)
    {
        if (null == neighbor)
        {
            return;
        }
        if (!neighbor.CurrentGemType.HasSameFlags(this.CurrentGemType))
        {
            return;
        }
        bool gemAdded = false;
        foreach (PossibleMatch possibleMatch in neighbor.PossibleMatches)
        {
            if (possibleMatch.AddGem(this))
            {
                gemAdded = true;
            }
        }
        if (!gemAdded)
        {
            PossibleMatch possibleMatch = new PossibleMatch(this.CurrentGemType.GetSameFlags(neighbor.CurrentGemType));
            possibleMatch.AddGem(this);
            possibleMatch.AddGem(neighbor);
            if (null != OnPossibleMatchAddedEvent)
            {
                OnPossibleMatchAddedEvent(this, possibleMatch);
            }
        }
    }

    private void Clear()
    {
        isActive = false;
        for (int i = possibleMatches.Count - 1; i >= 0; i--)
        {
            possibleMatches[i].RemoveGem(this);
        }
    }

    private void OnSwipeStart(SwipeComponent sender, Direction direction)
    {
        Clear();
        currentState = State.Moving;
    }

    private void OnSwipeEnd(SwipeComponent sender, Direction direction)
    {
        //Debug.Log("Moving gem(" + currentX + ", " + currentY + ") to " + direction);
        if (null != OnMovingToEvent)
        {
            OnMovingToEvent(this, direction);
        }
    }

    private void OnMovingStart(MovingComponent sender)
    {
        Clear();
        currentState = State.Moving;
    }

    private void OnMovingEnd(MovingComponent sender)
    {
        isActive = true;
        currentState = State.Idle;
        if (null != OnReadyEvent)
        {
            OnReadyEvent(this);
        }
    }

    public void OnMatch()
    {
        //Clear();
        currentState = State.Matched;
        isActive = false;
        possibleMatches.Clear();
        gemAnimator.SetTrigger("fadeout");
    }

    private void OnAppearOver()
    {
        //Debug.Log("OnAppearOver");
        isActive = true;
        if (null != OnReadyEvent)
        {
            OnReadyEvent(this);
        }
    }

    private void OnFadeoutOver()
    {
        gameObject.SetActive(false);
    }
}