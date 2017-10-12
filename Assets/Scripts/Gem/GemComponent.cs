using UnityEngine;

public abstract class GemComponent : MonoBehaviour
{
    public virtual void SetGemType(int type) { }
    public virtual void SetPosition(int x, int y, bool interpolate = false) { }
}