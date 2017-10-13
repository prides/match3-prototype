using UnityEngine;
using System.Collections.Generic;

public class GemManager : MonoBehaviour
{
    public GemController gemPrefab;

    public int rowCount = 7;
    public int columnCount = 7;

    private GemController[][] gems;
    private List<PossibleMatch> possibleMatches = new List<PossibleMatch>();

    private List<PossibleMatch> matchedMatches = new List<PossibleMatch>();

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

    private void LateUpdate()
    {
        if (matchedMatches.Count > 0)
        {
            List<int> needToCheckColums = new List<int>();
            foreach (PossibleMatch pm in matchedMatches)
            {
                if (pm.IsOver)
                {
                    continue;
                }
                foreach (GemController gem in pm.MatchedGems)
                {
                    gem.OnMatch();
                    if (!needToCheckColums.Contains(gem.CurrentX))
                    {
                        needToCheckColums.Add(gem.CurrentX);
                    }
                }
                pm.Clear();
            }
            foreach (int x in needToCheckColums)
            {
                int posToY = 0;
                int posFromY = 0;
                while (posFromY < rowCount)
                {
                    while (posToY < rowCount && gems[x][posToY].CurrentState != GemController.State.Matched)
                    {
                        posToY++;
                    }
                    if (posToY >= rowCount)
                    {
                        break;
                    }
                    posFromY = posToY + 1;
                    while (posFromY < rowCount && gems[x][posFromY].CurrentState == GemController.State.Matched)
                    {
                        posFromY++;
                    }
                    if (posFromY >= rowCount)
                    {
                        break;
                    }

                    gems[x][posFromY].SetPosition(x, posToY, true);
                    SwitchGems(x, posToY, x, posFromY);
                }
                posFromY = 0;
                while (posFromY < rowCount && gems[x][posFromY].CurrentState != GemController.State.Matched)
                {
                    posFromY++;
                }
                while (posFromY < rowCount)
                {
                    gems[x][posFromY] = CreateGem(x, posFromY, false);
                    posFromY++;
                }
            }
            matchedMatches.Clear();
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
        //Debug.Log("OnGemsMatch");
        if (!matchedMatches.Contains(sender))
        {
            matchedMatches.Add(sender);
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

        if (gem.LeftNeighbor != null)
        {
            gem.LeftNeighbor.RightNeighbor = gem;
        }
        if (gem.RightNeighbor != null)
        {
            gem.RightNeighbor.LeftNeighbor = gem;
        }
        if (gem.UpNeighbor != null)
        {
            gem.UpNeighbor.DownNeighbor = gem;
        }
        if (gem.DownNeighbor != null)
        {
            gem.DownNeighbor.UpNeighbor = gem;
        }
    }

    private GemController CreateGem(int x, int y, bool beggining = true)
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

        int type = Randomizer.Range(1, 6);
        if (unacceptableTypes.Count > 0)
        {
            bool acceptableTypeSelected = true;
            do
            {
                acceptableTypeSelected = true;
                type = Randomizer.Range(1, 6);
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
        gem.Init(beggining);
        return gem;
    }

    public void OnGemMove(GemController gem, Direction direction)
    {
        GemController neighbor = gem.GetNeighbor(direction);
        if (neighbor == null)
        {
            return;
        }
        int x1 = gem.CurrentX;
        int y1 = gem.CurrentY;
        int x2 = neighbor.CurrentX;
        int y2 = neighbor.CurrentY;

        SwitchGems(x1, y1, x2, y2);
        gem.SetPosition(x2, y2, true);
        neighbor.SetPosition(x1, y1, true);
        SwipeAction swipeAction = new SwipeAction(gem, neighbor);
        swipeAction.OnSwipeActionOver += OnSwipeOver;
        gem.CurrentSwipeAction = swipeAction;
        neighbor.CurrentSwipeAction = swipeAction;
    }

    private void OnSwipeOver(SwipeAction sender, GemController gem1, GemController gem2, bool isMatched)
    {
        if (gem1.CurrentSwipeAction == sender)
        {
            gem1.CurrentSwipeAction = null;
        }
        if (gem2.CurrentSwipeAction == sender)
        {
            gem2.CurrentSwipeAction = null;
        }
        if (isMatched)
        {
            return;
        }
        int x1 = gem1.CurrentX;
        int y1 = gem1.CurrentY;
        int x2 = gem2.CurrentX;
        int y2 = gem2.CurrentY;
        SwitchGems(x1, y1, x2, y2);
        gem1.SetPosition(x2, y2, true);
        gem2.SetPosition(x1, y1, true);
    }

    private void SwitchGems(int x1, int y1, int x2, int y2)
    {
        GemController tmpgem = gems[x1][y1];
        gems[x1][y1] = gems[x2][y2];
        gems[x2][y2] = tmpgem;
    }
}