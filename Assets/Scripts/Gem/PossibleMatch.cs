using System.Collections.Generic;

public class PossibleMatch
{
    public enum Direction
    {
        None,
        Horizontal,
        Vertical
    }

    public delegate void GemsMatchedEventDelegate(PossibleMatch sender, GemController[] matchedGems);
    public event GemsMatchedEventDelegate OnMatch;

    public delegate void SimplePossibleMatchDelegate(PossibleMatch sender);
    public event SimplePossibleMatchDelegate OnOver;

    private int matchType = 0;
    public int MatchType
    {
        get { return matchType; }
    }

    private Direction matchDirection = Direction.None;
    public Direction MatchDirection
    {
        get { return matchDirection; }
    }

    private List<GemController> matchedGems = new List<GemController>();

    public PossibleMatch(int type)
    {
        matchType = type;
    }

    public bool AddGem(GemController gem)
    {
        if (gem.CurrentGemType == matchType)
        {
            if (matchDirection == Direction.Horizontal && gem.CurrentX != matchedGems[0].CurrentX)
            {
                return false;
            }
            if (matchDirection == Direction.Vertical && gem.CurrentY != matchedGems[0].CurrentY)
            {
                return false;
            }
            if (matchedGems.Contains(gem))
            {
                return false;
            }
            if (matchedGems.Count == 1)
            {
                if (matchedGems[0].CurrentX == gem.CurrentX)
                {
                    matchDirection = Direction.Horizontal;
                }
                else if (matchedGems[0].CurrentY == gem.CurrentY)
                {
                    matchDirection = Direction.Vertical;
                }
            }
            matchedGems.Add(gem);
            gem.PossibleMatches.Add(this);
            return true;
        }
        return false;
    }

    public void RemoveGem(GemController gem)
    {
        matchedGems.Remove(gem);
        gem.PossibleMatches.Remove(this);
        if (matchedGems.Count == 1)
        {
            Clear();
        }
    }

    public void Merge(PossibleMatch other)
    {
        foreach (GemController gem in other.matchedGems)
        {
            AddGem(gem);
        }
        other.Clear();
    }

    public void Clear()
    {
        matchDirection = Direction.None;
        foreach (GemController gem in matchedGems)
        {
            gem.PossibleMatches.Remove(this);
        }
        matchedGems.Clear();
        if (null != OnOver)
        {
            OnOver(this);
        }
    }

    public bool CheckMatch()
    {
        if (matchedGems.Count >= 3)
        {
            if (null != OnMatch)
            {
                OnMatch(this, matchedGems.ToArray());
            }
            return true;
        }
        return false;
    }
}