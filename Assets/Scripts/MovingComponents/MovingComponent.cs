using UnityEngine;

public class MovingComponent : MovingComponentBase
{
    protected override void Update()
    {
        if (isDirty)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime / Duration);
            if (useLocalPosition)
            {
                Trans.localPosition = Vector3.Lerp(Trans.localPosition, Destination, progress);
            }
            else
            {
                Trans.position = Vector3.Lerp(Trans.position, Destination, progress);
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