using System;
using System.Linq;

public static class GemTypeExtensions
{
    public static void AddFlag(this GemType self, GemType other, out GemType result)
    {
        result = self | other;
    }

    public static void RemoveFlag(this GemType self, GemType other, out GemType result)
    {
        result = self & (~other);
    }

    public static bool HasFlag(this GemType self, GemType other)
    {
        return (self & other) == other;
    }

    public static bool HasSameFlags(this GemType self, GemType other)
    {
        return (self & other) != GemType.None;
    }

    public static GemType GetSameFlags(this GemType self, GemType other)
    {
        return self & other;
    }

    public static void ToogleFlag(this GemType self, GemType other, out GemType result)
    {
        result = self ^ other;
    }

    public static GemType Random(this GemType self)
    {
        GemType[] matching = Enum.GetValues(typeof(GemType)).Cast<GemType>().Where(v => self.HasFlag(v) && v != GemType.None).ToArray();
        if (matching.Length == 0)
        {
            return GemType.None;
        }
        return matching[Randomizer.Range(0, matching.Length)];
    }
}