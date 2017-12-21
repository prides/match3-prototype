using Match3Core;

namespace Match3Wrapper
{
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
}