using UnityEngine;
using System.Collections.Generic;

public class GemManager : MonoBehaviour
{
    public GemController gemPrefab;

    public int rowCount = 7;
    public int columnCount = 7;

    private GemController[][] gems;
    private List<PossibleMatch> possibleMatches = new List<PossibleMatch>();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        gems = new GemController[columnCount][];
        for (int x = 0; x < columnCount; x++)
        {
            gems[x] = new GemController[rowCount];
            for (int y = 0; y < rowCount; y++)
            {
                gems[x][y] = CreateGem(x, y);
            }
        }
    }

    private void OnDestroy()
    {
        if (null != gems)
        {
            for (int x = 0; x < columnCount; x++)
            {
                for (int y = 0; y < rowCount; y++)
                {
                    if (null != gems[x][y])
                    {
                        gems[x][y].OnReadyEvent -= OnGemReady;
                        gems[x][y].OnPossibleMatchAddedEvent -= OnPossibleMatchAdded;
                        gems[x][y].OnMovingToEvent -= OnGemMove;
                    }
                }
            }
        }
    }

    private void OnGemReady(GemController sender)
    {
        //Debug.Log("OnGemReady");
        SetGemNeighbor(sender);
    }

    private void OnPossibleMatchAdded(GemController sender, PossibleMatch possibleMatch)
    {
        if (null == possibleMatch)
        {
            Debug.LogError("possibleMatch is null");
            return;
        }
        possibleMatch.OnMatch += OnGemsMatch;
        possibleMatch.OnOver += OnPossibleMatchOver;
        possibleMatches.Add(possibleMatch);
    }

    private void OnPossibleMatchOver(PossibleMatch sender)
    {
        sender.OnMatch -= OnGemsMatch;
        sender.OnOver -= OnPossibleMatchOver;
        possibleMatches.Remove(sender);
    }

    private void OnGemsMatch(PossibleMatch sender, GemController[] matchGems)
    {
        Debug.Log("OnGemsMatch");
        sender.Clear();
        foreach (GemController gem in matchGems)
        {
            gem.OnMatch();
        }
    }

    private void SetGemNeighbor(GemController gem)
    {
        int x = gem.CurrentX;
        int y = gem.CurrentY;

        gem.LeftNeighbor = x - 1 >= 0 ? gems[x - 1][y] : null;
        gem.RightNeighbor = x + 1 < columnCount ? gems[x + 1][y] : null;
        gem.UpNeighbor = y + 1 < rowCount ? gems[x][y + 1] : null;
        gem.DownNeighbor = y - 1 >= 0 ? gems[x][y - 1] : null;
    }

    private GemController CreateGem(int x, int y)
    {
        GemController gem = Instantiate(gemPrefab);
        gem.transform.parent = this.transform;
        gem.SetPosition(x, y);

        List<int> unacceptableTypes = new List<int>();
        GemController xm1Neighbor = x - 1 >= 0 ? gems[x - 1][y] : null;
        GemController xm2Neighbor = x - 2 >= 0 ? gems[x - 2][y] : null;
        if ((xm1Neighbor != null && xm2Neighbor != null) && (xm1Neighbor.CurrentGemType == xm2Neighbor.CurrentGemType))
        {
            unacceptableTypes.Add(xm1Neighbor.CurrentGemType);
        }

        GemController ym1Neighbor = y - 1 >= 0 ? gems[x][y - 1] : null;
        GemController ym2Neighbor = y - 2 >= 0 ? gems[x][y - 1] : null;
        if ((ym1Neighbor != null && ym2Neighbor != null) && (ym1Neighbor.CurrentGemType == ym2Neighbor.CurrentGemType))
        {
            unacceptableTypes.Add(ym1Neighbor.CurrentGemType);
        }

        int type = Randomizer.Range(1, 5);
        if (unacceptableTypes.Count > 0)
        {
            bool acceptableTypeSelected = true;
            do
            {
                acceptableTypeSelected = true;
                type = Randomizer.Range(1, 5);
                foreach (int unacceptableType in unacceptableTypes)
                {
                    if (unacceptableType == type)
                    {
                        acceptableTypeSelected = false;
                    }
                }
            } while (!acceptableTypeSelected);
        }

        gem.SetGemType(type);
        gem.OnReadyEvent += OnGemReady;
        gem.OnPossibleMatchAddedEvent += OnPossibleMatchAdded;
        gem.OnMovingToEvent += OnGemMove;
        gem.Init();
        return gem;
    }

    public void OnGemMove(GemController gem, Direction direction)
    {
        int x = gem.CurrentX;
        int y = gem.CurrentY;
        switch (direction)
        {
            case Direction.Up:
                if (gem.CurrentY < rowCount - 1)
                {
                    SwitchGems(x, y, x, y + 1);
                    gem.SetPosition(x, y + 1, true);
                    gem.UpNeighbor.SetPosition(x, y, true);
                }
                break;
            case Direction.Right:
                if (gem.CurrentX < columnCount - 1)
                {
                    SwitchGems(x, y, x + 1, y);
                    gem.SetPosition(x + 1, y, true);
                    gem.RightNeighbor.SetPosition(x, y, true);
                }
                break;
            case Direction.Down:
                if (gem.CurrentY > 0)
                {
                    SwitchGems(x, y, x, y - 1);
                    gem.SetPosition(x, y - 1, true);
                    gem.DownNeighbor.SetPosition(x, y, true);
                }
                break;
            case Direction.Left:
                if (gem.CurrentX > 0)
                {
                    SwitchGems(x, y, x - 1, y);
                    gem.SetPosition(x - 1, y, true);
                    gem.LeftNeighbor.SetPosition(x, y, true);
                }
                break;
        }
    }

    private void SwitchGems(int x1, int y1, int x2, int y2)
    {
        GemController tmpgem = gems[x1][y1];
        gems[x1][y1] = gems[x2][y2];
        gems[x2][y2] = tmpgem;
    }
}