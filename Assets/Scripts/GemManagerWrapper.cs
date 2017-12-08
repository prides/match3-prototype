using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Match3Core;

public class GemManagerWrapper : MonoBehaviour
{
    [System.Serializable]
    public class PossibleMoveWrapper
    {
        public Vector2 gemPos;
        public Vector2 movePos;
        private PossibleMove instance;

        public PossibleMoveWrapper(PossibleMove instance)
        {
            this.instance = instance;
            this.gemPos = new Vector2(instance.Key.Position.x, instance.Key.Position.y);
            this.movePos = new Vector2(instance.MatchablePosition.x, instance.MatchablePosition.y);
        }

        public void Move()
        {
            ((GemControllerWrapper)instance.Key.tag).MoveTo(DirectionHelper.GetDirectionByPosition(instance.Key.Position, instance.MatchablePosition));
        }
    }

    public delegate void EventDelegateWithBool(object sender, bool value);
    public event EventDelegateWithBool OnReadyChangedEvent;

    public delegate void EventDelegateWithGemPoints(object sender, GemPoint[] gemPoints);
    public event EventDelegateWithGemPoints OnGemPointsReceivedEvent;

    public int rowCount = 7;
    public int columnCount = 7;

    public GemControllerWrapper gemPrefab;

    private GemManager gemManagerInstance;

    [SerializeField]
    [ReadOnly]
    private List<PossibleMoveWrapper> possibleMoves = new List<PossibleMoveWrapper>();
    public List<PossibleMoveWrapper> PossibleMoves
    {
        get { return possibleMoves; }
    }

    private bool isReady = false;
    public bool IsReady
    {
        get { return isReady; }
        private set
        {
            if (isReady == value)
            {
                return;
            }
            isReady = value;
            OnReadyChanged(isReady);
        }
    }

    private void Awake()
    {
        setReadyCoroutine = WaitAndSetReady(0.2f);
        gemManagerInstance = new GemManager(rowCount, columnCount);
        gemManagerInstance.OnGemCreated += OnGemCreated;
        gemManagerInstance.OnGemMatch += OnGemMatch;
        gemManagerInstance.OnReadyStateChanged += OnReadyStateChanged;
        gemManagerInstance.OnPossibleMoveCreate += OnPossibleMoveCreate;
        gemManagerInstance.Init();
    }

    private IEnumerator setReadyCoroutine;
    private void OnReadyStateChanged(GemManager sender, bool value)
    {
        if (value)
        {
            StopCoroutine(setReadyCoroutine);
            setReadyCoroutine = WaitAndSetReady(0.2f);
            StartCoroutine(setReadyCoroutine);
        }
        else
        {
            StopCoroutine(setReadyCoroutine);
            IsReady = value;
        }
    }

    private IEnumerator WaitAndSetReady(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        IsReady = true;
    }

    private void OnReadyChanged(bool value)
    {
        if (value)
        {
            if (possibleMoves.Count == 0)
            {
                gemManagerInstance.ShuffleGems();
                isReady = false;
                return;
            }
        }
        Debug.Log("GemManager ready state changed. value:" + value);
        if (null != OnReadyChangedEvent)
        {
            OnReadyChangedEvent(this, isReady);
        }
    }

    private List<GemPoint> gemPoints = new List<GemPoint>();
    private void OnGemMatch(GemManager sender, GemController instance)
    {
        if (instance.CurrentGemType == GemType.HitType)
        {
            return;
        }
        if (gemPoints.Count == 0)
        {
            StartCoroutine(SendGemPointsEventAtOnce());
        }
        gemPoints.Add(new GemPoint(instance.CurrentGemType, 1.0f));
    }

    private IEnumerator SendGemPointsEventAtOnce()
    {
        yield return new WaitForEndOfFrame();
        if (null != OnGemPointsReceivedEvent)
        {
            OnGemPointsReceivedEvent(this, gemPoints.ToArray());
        }
        gemPoints.Clear();
    }

    private void OnPossibleMoveCreate(PossibleMove possibleMove)
    {
        PossibleMoveWrapper posmove = new PossibleMoveWrapper(possibleMove);
        possibleMove.tag = posmove;
        possibleMove.OnOver += OnPossibleMoveOver;
        possibleMoves.Add(posmove);
    }

    private void OnPossibleMoveOver(PossibleMove sender)
    {
        PossibleMoveWrapper posmove = (PossibleMoveWrapper)sender.tag;
        possibleMoves.Remove(posmove);
    }

    private void OnGemCreated(GemManager sender, GemController createdGemInstance)
    {
        GemControllerWrapper gem = Instantiate(gemPrefab);
        gem.SetInstance(createdGemInstance);
        gem.transform.parent = this.transform;
    }

    private void OnDestroy()
    {
        gemManagerInstance.OnGemCreated -= OnGemCreated;
        gemManagerInstance.OnGemMatch -= OnGemMatch;
        gemManagerInstance.OnReadyStateChanged -= OnReadyStateChanged;
        gemManagerInstance.OnPossibleMoveCreate -= OnPossibleMoveCreate;
        gemManagerInstance.Deinit();
    }

    private void LateUpdate()
    {
        gemManagerInstance.CheckMatchedMatches();
    }
}