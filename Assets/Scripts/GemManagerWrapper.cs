using UnityEngine;
using System.Collections.Generic;
using Match3Core;

public class GemManagerWrapper : MonoBehaviour
{
    public int rowCount = 7;
    public int columnCount = 7;

    public GemControllerWrapper gemPrefab;

    private GemManager gemManagerInstance;

    private void Awake()
    {
        gemManagerInstance = new GemManager(rowCount, columnCount);
        gemManagerInstance.OnGemCreated += OnGemCreated;
        gemManagerInstance.Init();
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
        gemManagerInstance.DeInit();
    }

    private void LateUpdate()
    {
        gemManagerInstance.CheckMatchedMatches();
    }
}