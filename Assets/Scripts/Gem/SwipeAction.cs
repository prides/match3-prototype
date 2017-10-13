using System;
using System.Collections.Generic;
using UnityEngine;

public class SwipeAction
{
    private class SwipeInfo
    {
        internal GemController gem = null;
        internal bool isOver = false;
        internal bool isMatched = false;

        internal SwipeInfo(GemController gem, bool isOver, bool isMatched)
        {
            this.gem = gem;
            this.isOver = isOver;
            this.isMatched = isMatched;
        }
    }

    public delegate void SwipeActionEventDelegate(SwipeAction sender, GemController gem1, GemController gem2, bool result);
    public event SwipeActionEventDelegate OnSwipeActionOver;

    private List<SwipeInfo> swipingGems = new List<SwipeInfo>();

    public SwipeAction(GemController gem1, GemController gem2)
    {
        swipingGems.Add(new SwipeInfo(gem1, false, false));
        swipingGems.Add(new SwipeInfo(gem2, false, false));
    }

    public void SetGemSwipeResult(GemController gem, bool isMatched)
    {
        Debug.Log("gem(" + gem.CurrentX + "," + gem.CurrentY + ") is " + (isMatched ? "matched" : "not matched"));
        for (int i = 0; i < swipingGems.Count; i++)
        {
            if (swipingGems[i].gem == gem)
            {
                swipingGems[i].isMatched = isMatched;
                swipingGems[i].isOver = true;
                break;
            }
        }

        CheckGemsSwipeEnded();
    }

    private void CheckGemsSwipeEnded()
    {
        bool isOver = true;
        bool isMatched = false;
        foreach(SwipeInfo si in swipingGems)
        {
            if (si.isMatched)
            {
                isMatched = true;
            }
            if (!si.isOver)
            {
                isOver = false;
            }
        }
        if (isOver && OnSwipeActionOver != null)
        {
            Debug.Log("swipe is over and is " + (isMatched ? "matched" : "not matched"));
            OnSwipeActionOver(this, swipingGems[0].gem, swipingGems[1].gem, isMatched);
        }
    }
}
