using UnityEngine;
using System.Collections.Generic;

public class GemManager : MonoBehaviour
{
    public GemController gemPrefab;

    public int rowCount = 7;
    public int columnCount = 7;

    private GemController[][] gems;

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
                        gems[x][y].OnMatchEvent -= OnGemsMatch;
                    }
                }
            }
        }
    }

    private void OnGemReady(GemController sender)
    {
        SetGemNeighbor(sender);
    }

    private void OnGemsMatch(GemController sender, GemController[] matchGems)
    {
        Debug.Log("OnGemsMatch");
        foreach(GemController gem in matchGems)
        {
            gem.OnMatch();
            gem.LeftNeighbor = null;
            gem.RightNeighbor = null;
            gem.UpNeighbor = null;
            gem.DownNeighbor = null;
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

        //List<int> unacceptableTypes = new List<int>();
        //GemController xm1Neighbor = x - 1 >= 0 ? gems[x - 1][y] : null;
        //GemController xm2Neighbor = x - 2 >= 0 ? gems[x - 2][y] : null;
        //if ((xm1Neighbor != null && xm2Neighbor != null) && (xm1Neighbor.CurrentGemType == xm2Neighbor.CurrentGemType))
        //{
        //    unacceptableTypes.Add(xm1Neighbor.CurrentGemType);
        //}

        //GemController ym1Neighbor = y - 1 >= 0 ? gems[x][y - 1] : null;
        //GemController ym2Neighbor = y - 2 >= 0 ? gems[x][y - 1] : null;
        //if ((ym1Neighbor != null && ym2Neighbor != null) && (ym1Neighbor.CurrentGemType == ym2Neighbor.CurrentGemType))
        //{
        //    unacceptableTypes.Add(ym1Neighbor.CurrentGemType);
        //}

        int type = Randomizer.Range(1, 5);
        //if (unacceptableTypes.Count > 0)
        //{
        //    bool acceptableTypeSelected = true;
        //    do
        //    {
        //        acceptableTypeSelected = true;
        //        type = Randomizer.Range(1, 5);
        //        foreach (int unacceptableType in unacceptableTypes)
        //        {
        //            if (unacceptableType == type)
        //            {
        //                acceptableTypeSelected = false;
        //            }
        //        }
        //    } while (!acceptableTypeSelected);
        //}

        gem.SetGemType(type);
        gem.OnReadyEvent += OnGemReady;
        gem.OnMatchEvent += OnGemsMatch;
        gem.Init();
        return gem;
    }
}