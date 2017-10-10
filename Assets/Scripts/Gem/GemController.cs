using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : GemComponent
{
    public delegate void SimpleGemEventDelegate(GemController sender);
    public event SimpleGemEventDelegate OnReadyEvent;

    public delegate void GemEventWithArrayDelegate(GemController sender, GemController[] array);
    public event GemEventWithArrayDelegate OnMatchEvent;

    [SerializeField]
    private GemComponent[] gemComponents;

    [SerializeField]
    private Animator gemAnimator;

    [SerializeField]
    [ReadOnly]
    private int currentGemType = 0;
    public int CurrentGemType
    {
        get { return currentGemType; }
        private set { currentGemType = value; }
    }

    [SerializeField]
    [ReadOnly]
    private int currentX = 0;
    public int CurrentX
    {
        get { return currentX; }
        private set { currentX = value; }
    }

    [SerializeField]
    [ReadOnly]
    private int currentY = 0;
    public int CurrentY
    {
        get { return currentY; }
        private set { currentY = value; }
    }

    [SerializeField]
    [ReadOnly]
    private bool isActive = false;

    #region neighbor
    private GemController leftNeighbor = null;
    private GemController rightNeighbor = null;
    private GemController upNeighbor = null;
    private GemController downNeighbor = null;

    private bool neighborChanged = false;
    private bool leftNeighborChanged = false;
    private bool rightNeighborChanged = false;
    private bool upNeighborChanged = false;
    private bool downNeighborChanged = false;

    public GemController LeftNeighbor
    {
        get { return leftNeighbor; }
        set
        {
            leftNeighbor = value;
            neighborChanged = leftNeighborChanged = true;
        }
    }

    public GemController RightNeighbor
    {
        get { return rightNeighbor; }
        set
        {
            rightNeighbor = value;
            neighborChanged = rightNeighborChanged = true;
        }
    }

    public GemController UpNeighbor
    {
        get { return upNeighbor; }
        set
        {
            upNeighbor = value;
            neighborChanged = upNeighborChanged = true;
        }
    }

    public GemController DownNeighbor
    {
        get { return downNeighbor; }
        set
        {
            downNeighbor = value;
            neighborChanged = downNeighborChanged = true;
        }
    }
    #endregion

    public override void SetGemType(int type)
    {
        currentGemType = type;
        foreach (GemComponent gemComponent in gemComponents)
        {
            gemComponent.SetGemType(type);
        }
    }

    public void SetPosition(int x, int y, bool interpolate = false)
    {
        CurrentX = x;
        CurrentY = y;
        if (!interpolate)
        {
            transform.localPosition = new Vector3(x, y);
        }
    }

    public void OnMatch()
    {
        gemAnimator.SetTrigger("fadeout");
    }

    public void Init()
    {
        StartCoroutine(WaitAndTrigger(Randomizer.Range(0.0f, 0.75f), "appear"));
    }

    private IEnumerator WaitAndTrigger(float wait, string triggerName)
    {
        yield return new WaitForSeconds(wait);
        gemAnimator.SetTrigger(triggerName);
    }

    private void LateUpdate()
    {
        if (neighborChanged)
        {
            OnNeighborChanged();
        }
    }

    private void OnNeighborChanged()
    {
        List<GemController> matchedGems = new List<GemController>();
        matchedGems.Add(this);
        GemController neighbor = leftNeighbor;
        while (neighbor != null && neighbor.isActive)
        {
            if (neighbor.CurrentGemType == this.CurrentGemType)
            {
                matchedGems.Add(neighbor);
            }
            else
            {
                break;
            }
            neighbor = neighbor.leftNeighbor;
        }

        neighbor = rightNeighbor;
        while (neighbor != null && neighbor.isActive)
        {
            if (neighbor.CurrentGemType == this.CurrentGemType)
            {
                matchedGems.Add(neighbor);
            }
            else
            {
                break;
            }
            neighbor = neighbor.rightNeighbor;
        }

        if (matchedGems.Count >= 3)
        {
            if (null != OnMatchEvent)
            {
                OnMatchEvent(this, matchedGems.ToArray());
            }
        }

        matchedGems.Clear();
        matchedGems.Add(this);
        neighbor = upNeighbor;
        while (neighbor != null && neighbor.isActive)
        {
            if (neighbor.CurrentGemType == this.CurrentGemType)
            {
                matchedGems.Add(neighbor);
            }
            else
            {
                break;
            }
            neighbor = neighbor.upNeighbor;
        }

        neighbor = downNeighbor;
        while (neighbor != null && neighbor.isActive)
        {
            if (neighbor.CurrentGemType == this.CurrentGemType)
            {
                matchedGems.Add(neighbor);
            }
            else
            {
                break;
            }
            neighbor = neighbor.downNeighbor;
        }

        if (matchedGems.Count >= 3)
        {
            if (null != OnMatchEvent)
            {
                OnMatchEvent(this, matchedGems.ToArray());
            }
        }

        neighborChanged = false;
    }

    private void OnAppearOver()
    {
        isActive = true;
        if (null != OnReadyEvent)
        {
            OnReadyEvent(this);
        }
    }

    private void OnFadeoutOver()
    {
        gameObject.SetActive(false);
    }
}