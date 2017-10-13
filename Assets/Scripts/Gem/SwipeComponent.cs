using System;
using UnityEngine;

public class SwipeComponent : GemComponent
{
    public delegate void SimpleSwipeComponentEvent(SwipeComponent sender, Direction direction);
    public event SimpleSwipeComponentEvent OnSwipeStartEvent;
    public event SimpleSwipeComponentEvent OnSwipeEndEvent;

    public enum State
    {
        Idle,
        Touched,
        Swiping
    }

    private Transform trans;
    public Transform Trans
    {
        get
        {
            if (null == trans)
            {
                trans = transform;
            }
            return trans;
        }
    }

    private Camera currentCamera;
    public Camera CurrentCamera
    {
        get
        {
            if (currentCamera == null)
            {
                currentCamera = Camera.main;
            }
            return currentCamera;
        }
    }

    private Direction currentConstrains = Direction.None;
    public Direction CurrentConstrains
    {
        get { return currentConstrains; }
        set { currentConstrains = value; }
    }

    private Vector3 basePosition = Vector3.zero;
    public Vector3 BasePosition
    {
        get { return basePosition; }
        set { basePosition = value; }
    }

    private float permitedDistance = 1.0f;
    public float PermitedDistance
    {
        get { return permitedDistance; }
        set { permitedDistance = value; }
    }

    [SerializeField]
    private float deadZoneDistance = 0.2f;

    [ReadOnly]
    [SerializeField]
    private Direction currentDirection = Direction.None;

    private State currentState = State.Idle;
    private Vector3 mouseDownWorldPosition = Vector3.zero;

    public override void SetPosition(int x, int y, bool interpolate = false)
    {
        BasePosition = new Vector3(x, y);
    }

    public void AddConstrain(Direction constrain)
    {
        currentConstrains |= constrain;
    }

    public void RemoveConstrain(Direction constrain)
    {
        currentConstrains = currentConstrains & ~constrain;
    }

    private void OnMouseDown()
    {
        if (currentState != State.Idle)
        {
            SetToDefault();
        }
        mouseDownWorldPosition = CurrentCamera.ScreenToWorldPoint(Input.mousePosition);
        currentState = State.Touched;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (currentState == State.Touched || currentState == State.Swiping)
            {
                OnDrag();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (currentState == State.Swiping)
            {
                if (OnSwipeEndEvent != null)
                {
                    OnSwipeEndEvent(this, currentDirection);
                }
            }
            SetToDefault();
        }
    }

    private void OnDrag()
    {
        if (currentState != State.Touched && currentState != State.Swiping)
        {
            return;
        }
        Vector3 mouseWorldPosition = CurrentCamera.ScreenToWorldPoint(Input.mousePosition);
        float mouseDistance = Vector3.Distance(mouseDownWorldPosition, mouseWorldPosition);
        Vector3 mouseDiff = mouseWorldPosition - mouseDownWorldPosition;
        if (mouseDistance > deadZoneDistance && currentState == State.Touched)
        {
            currentState = State.Swiping;
            if (Mathf.Abs(mouseDiff.x) > Mathf.Abs(mouseDiff.y))
            {
                if (mouseDiff.x > 0)
                {
                    currentDirection = Direction.Right;
                }
                else
                {
                    currentDirection = Direction.Left;
                }
            }
            else
            {
                if (mouseDiff.y > 0)
                {
                    currentDirection = Direction.Up;
                }
                else
                {
                    currentDirection = Direction.Down;
                }
            }
            if ((currentConstrains & currentDirection) != Direction.None)
            {
                SetToDefault();
                return;
            }
            if (OnSwipeStartEvent != null)
            {
                OnSwipeStartEvent(this, currentDirection);
            }
        }
        if (currentState == State.Swiping)
        {
            Vector3 position = Trans.position;
            switch (currentDirection)
            {
                case Direction.Left:
                    position.x = mouseDiff.x <= -permitedDistance ? basePosition.x - permitedDistance : mouseDiff.x < 0.0f ? mouseWorldPosition.x : basePosition.x;
                    break;
                case Direction.Right:
                    position.x = mouseDiff.x >= permitedDistance ? basePosition.x + permitedDistance : mouseDiff.x > 0.0f ? mouseWorldPosition.x : basePosition.x;
                    break;
                case Direction.Up:
                    position.y = mouseDiff.y >= permitedDistance ? basePosition.y + permitedDistance : mouseDiff.y > 0.0f ? mouseWorldPosition.y : basePosition.y;
                    break;
                case Direction.Down:
                    position.y = mouseDiff.y <= -permitedDistance ? basePosition.y - permitedDistance : mouseDiff.y < 0.0f ? mouseWorldPosition.y : basePosition.y;
                    break;
            }
            Trans.position = position;
        }
    }

    private void SetToDefault()
    {
        currentState = State.Idle;
        currentDirection = Direction.None;
        mouseDownWorldPosition = Vector3.zero;
    }
}