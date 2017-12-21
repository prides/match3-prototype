using UnityEngine;

public class SimpleJumpingComponent : MovingComponentBase
{
    [SerializeField]
    protected float height = 2.0f;
    public float Height
    {
        get { return height; }
        set { height = value; }
    }

    protected override void Update()
    {
        if (isDirty)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime / Duration);
            Vector3 currentPosition = useLocalPosition ? Trans.localPosition : Trans.position;
            currentPosition.x = Mathf.Lerp(currentPosition.x, Destination.x, progress);
            currentPosition.y = Mathf.Lerp(startPosition.y, startPosition.y + Height, progress < 0.5f ? progress * 2.0f : (1.0f - progress) * 2.0f);
            currentPosition.z = Mathf.Lerp(currentPosition.z, Destination.z, progress);
            if (useLocalPosition)
            {
                Trans.localPosition = currentPosition;
            }
            else
            {
                Trans.position = currentPosition;
            }
            if (progress >= 1.0f)
            {
                isDirty = false;
                state = MovingState.Idle;
                if (null != onMovingEnd)
                {
                    onMovingEnd();
                    onMovingEnd = null;
                }
            }
        }
    }
}