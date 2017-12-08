using UnityEngine;
using System.Collections.Generic;
using Match3Core;

public class BrokenGemsQueue : MonoBehaviour
{
    [SerializeField]
    private BrokenGemController brokenGemPrefab;

    private Queue<BrokenGemController> brokenGemsQueue = new Queue<BrokenGemController>();

    public void AddGemPoints(GemPoint[] gemPoints)
    {
        Dictionary<GemType, List<GemPoint>> gems = new Dictionary<GemType, List<GemPoint>>();
        foreach (GemPoint gemPoint in gemPoints)
        {
            if (!gems.ContainsKey(gemPoint.type))
            {
                gems.Add(gemPoint.type, new List<GemPoint>());
            }
            gems[gemPoint.type].Add(gemPoint);
        }

        foreach (GemType key in gems.Keys)
        {
            BrokenGemController brokenGem = Instantiate(brokenGemPrefab);
            brokenGem.transform.parent = transform;
            brokenGem.SetPosition(brokenGemsQueue.Count);
            brokenGem.GemPoints = gems[key].ToArray();
            brokenGem.Appear(null);
            brokenGemsQueue.Enqueue(brokenGem);
        }
    }

    public GemPoint[] GetGemPoints()
    {
        BrokenGemController brokenGem = brokenGemsQueue.Count > 0 ? brokenGemsQueue.Dequeue() : null;
        if (null == brokenGem)
        {
            return null;
        }
        brokenGem.Disappear(() => {
            int index = 0;
            foreach (BrokenGemController bg in brokenGemsQueue)
            {
                bg.SetPosition(index++);
            }
        });
        return brokenGem.GemPoints;
    }
}