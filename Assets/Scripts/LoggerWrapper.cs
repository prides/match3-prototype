using UnityEngine;

public class LoggerWrapper : MonoBehaviour
{
    public bool listenMessage = true;
    public bool listenWarning = true;
    public bool listenError = true;
    private void Awake()
    {
        if (listenMessage)
            Match3Core.Logger.Instance.OnDebugMessage += OnDebugMessage;
        if (listenWarning)
            Match3Core.Logger.Instance.OnWarningMessage += OnWarningMessage;
        if (listenError)
            Match3Core.Logger.Instance.OnErrorMessage += OnErrorMessage;
    }

    private void OnDestroy()
    {
        if (listenMessage)
            Match3Core.Logger.Instance.OnDebugMessage -= OnDebugMessage;
        if (listenWarning)
            Match3Core.Logger.Instance.OnWarningMessage -= OnWarningMessage;
        if (listenError)
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