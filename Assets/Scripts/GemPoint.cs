using Match3Core;

[System.Serializable]
public struct GemPoint
{
    public GemType type;
    public float weight;

    public GemPoint(GemType type, float weight)
    {
        this.type = type;
        this.weight = weight;
    }
}