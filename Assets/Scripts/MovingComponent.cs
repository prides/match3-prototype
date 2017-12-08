using UnityEngine;

public class MovingComponent : GemComponent
{
    public delegate void SimpleMovingComponentEvent(MovingComponent sender);
    public event SimpleMovingComponentEvent OnMovingStartEvent;
    public event SimpleMovingComponentEvent OnMovingEndEvent;

    public enum MovingState {
        None,
        Idle,
        Moving
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

    private Vector3 destination;
    public Vector3 Destination
    {
        get { return destination; }
        set
        {
            destination = value;
            isDirty = true;
            progress = 0.0f;
        }
    }

    private bool isDirty = false;
    private MovingState state = MovingState.Idle;
    private float progress = 0.0f;

    public override void SetPosition(Vector3 position, bool interpolate = false)
    {
        if (interpolate)
        {
            Destination = position;
            if (state == MovingState.Idle)
            {
                if (null != OnMovingStartEvent)
                {
                    OnMovingStartEvent(this);
                }
                state = MovingState.Moving;
            }
        }
        else
        {
            Trans.localPosition = position;
        }
    }

    private void Update()
    {
        if (isDirty)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime * 3.0f);
            Trans.localPosition = Vector3.Lerp(Trans.localPosition, destination, progress);
            if (progress >= 1.0f)
            {
                isDirty = false;
                state = MovingState.Idle;
                if (null != OnMovingEndEvent)
                {
                    OnMovingEndEvent(this);
                }
            }
        }
    }
}