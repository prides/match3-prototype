using UnityEngine;

public class LoggerWrapper : MonoBehaviour
{
    private void Awake()
    {
        Match3Core.Logger.Instance.OnDebugMessage += OnDebugMessage;
        Match3Core.Logger.Instance.OnWarningMessage += OnWarningMessage;
        Match3Core.Logger.Instance.OnErrorMessage += OnErrorMessage;
    }

    private void OnDestroy()
    {
        Match3Core.Logger.Instance.OnDebugMessage -= OnDebugMessage;
        Match3Core.Logger.Instance.OnWarningMessage -= OnWarningMessage;
        Match3Core.Logger.Instance.OnErrorMessage -= OnErrorMessage;
    }

    private void OnDebugMessage(string message)
    {
        Debug.Log(message);
    }

    private void OnWarningMessage(string message)
    {
        Debug.LogWarning(message);
    }

    private void OnErrorMessage(string message)
    {
        Debug.LogError(message);
    }
}