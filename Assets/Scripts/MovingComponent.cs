using UnityEngine;

public class MovingComponent : MonoBehaviour
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

    private void Update()
    {
        if (isDirty)
        {
            if (state == MovingState.Idle)
            {
                if (null != OnMovingStartEvent)
                {
                    OnMovingStartEvent(this);
                }
                state = MovingState.Moving;
            }
            progress = Mathf.Clamp01(progress + Time.deltaTime * 2.0f);
            Trans.position = Vector3.Lerp(Trans.position, destination, progress);
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

    private Vector3 mouseDownPosition = Vector3.zero;
    private void OnMouseDown()
    {
        mouseDownPosition = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        if (Vector3.Distance(mouseDownPosition, Input.mousePosition) > 0.25f)
        {
            Vector3 positionDiff = Input.mousePosition - mouseDownPosition;
            if (Mathf.Abs(positionDiff.x) > Mathf.Abs(positionDiff.y))
            {
                if (positionDiff.x > 0)
                {
                    GemManager.instance.MoveGem(GetComponent<GemController>(), 1);
                }
                else
                {
                    GemManager.instance.MoveGem(GetComponent<GemController>(), 3);
                }
            }
            else
            {
                if (positionDiff.y > 0)
                {
                    GemManager.instance.MoveGem(GetComponent<GemController>(), 0);
                }
                else
                {
                    GemManager.instance.MoveGem(GetComponent<GemController>(), 2);
                }
            }
        }
        mouseDownPosition = Vector2.zero;
    }
}