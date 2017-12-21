using UnityEngine;

public abstract class MovingComponentBase : MonoBehaviour
{
    public delegate void SimpleMovingComponentEvent(MovingComponent sender);

    public enum MovingState
    {
        None,
        Idle,
        Moving
    }

    [SerializeField]
    protected bool useLocalPosition = false;

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

    private float duration = 0.33f;
    public float Duration
    {
        get { return duration; }
        set { duration = value; }
    }

    protected bool isDirty = false;
    protected MovingState state = MovingState.Idle;
    protected float progress = 0.0f;

    protected SimpleCallbackDelegate onMovingEnd;
    protected Vector3 startPosition;

    public void MoveTo(Vector3 position, bool interpolate = false, SimpleCallbackDelegate onMovingEndCallback = null)
    {
        if (interpolate)
        {
            startPosition = useLocalPosition ? Trans.localPosition : Trans.position;
            onMovingEnd = onMovingEndCallback;
            Destination = position;
            if (state == MovingState.Idle)
            {
                state = MovingState.Moving;
            }
        }
        else
        {
            if (useLocalPosition)
            {
                Trans.localPosition = position;
            }
            else
            {
                Trans.position = position;
            }
        }
    }

    protected abstract void Update();
}