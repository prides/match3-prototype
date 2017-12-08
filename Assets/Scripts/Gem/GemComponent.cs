using UnityEngine;
using Match3Core;

public abstract class GemComponent : MonoBehaviour
{
    public virtual void SetGemType(GemType type) { }
    public virtual void SetPosition(Vector3 position, bool interpolate = false) { }
    public virtual void SetGemSpecialType(GemSpecialType specialType) { }
}