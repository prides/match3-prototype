using UnityEngine;
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
    }

    public int rowCount = 7;
    public int columnCount = 7;

    public GemControllerWrapper gemPrefab;

    private GemManager gemManagerInstance;

    [SerializeField]
    [ReadOnly]
    private List<PossibleMoveWrapper> possibleMoves = new List<PossibleMoveWrapper>();

    private void Awake()
    {
        gemManagerInstance = new GemManager(rowCount, columnCount);
        gemManagerInstance.OnGemCreated += OnGemCreated;
        gemManagerInstance.OnPossibleMoveCreate += OnPossibleMoveCreate;
        gemManagerInstance.Init();
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

    private void OnGemCreated(GemController createdGemInstance)
    {
        GemControllerWrapper gem = Instantiate(gemPrefab);
        gem.SetInstance(createdGemInstance);
        gem.transform.parent = this.transform;
    }

    private void OnDestroy()
    {
        gemManagerInstance.OnGemCreated -= OnGemCreated;
        gemManagerInstance.Deinit();
    }

    private void LateUpdate()
    {
        gemManagerInstance.CheckMatchedMatches();
    }
}