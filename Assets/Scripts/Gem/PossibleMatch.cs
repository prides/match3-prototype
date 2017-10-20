using System.Collections.Generic;

public class PossibleMatch
{
    public enum Line
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2,
        Cross = 3
    }

    public delegate void GemsMatchedEventDelegate(PossibleMatch sender, GemController[] matchedGems);
    public event GemsMatchedEventDelegate OnMatch;

    public delegate void SimplePossibleMatchDelegate(PossibleMatch sender);
    public event SimplePossibleMatchDelegate OnOver;

    private GemType matchType = 0;
    public GemType MatchType
    {
        get { return matchType; }
    }

    private Line matchDirection = Line.None;
    public Line MatchDirection
    {
        get { return matchDirection; }
    }

    private List<GemController> matchedGems = new List<GemController>();
    public List<GemController> MatchedGems
    {
        get { return matchedGems; }
    }

    private bool isOver = false;
    public bool IsOver
    {
        get { return isOver; }
    }

    public PossibleMatch(GemType type)
    {
        matchType = type;
    }

    public bool AddGem(GemController gem)
    {
        if (gem.CurrentGemType.HasSameFlags(matchType))
        {
            if (matchDirection == Line.Horizontal && gem.CurrentX != matchedGems[0].CurrentX)
            {
                return false;
            }
            if (matchDirection == Line.Vertical && gem.CurrentY != matchedGems[0].CurrentY)
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
                    matchDirection = Line.Horizontal;
                }
                else if (matchedGems[0].CurrentY == gem.CurrentY)
                {
                    matchDirection = Line.Vertical;
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
        this.matchDirection |= other.matchDirection;
        foreach (GemController gem in other.matchedGems)
        {
            AddGem(gem);
        }
        other.Clear();
    }

    public void Clear()
    {
        isOver = true;
        matchDirection = Line.None;
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

    public bool IsMatched()
    {
        return matchedGems.Count >= 3;
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