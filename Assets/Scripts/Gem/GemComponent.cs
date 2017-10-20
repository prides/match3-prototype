using UnityEngine;
using Match3Core;

public abstract class GemComponent : MonoBehaviour
{
    public virtual void SetGemType(GemType type) { }
    public virtual void SetPosition(int x, int y, bool interpolate = false) { }
}