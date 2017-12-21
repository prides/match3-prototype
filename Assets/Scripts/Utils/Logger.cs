using UnityEngine;

namespace Utils
{
    public class Logger : MonoBehaviour
    {
        public bool listenMessage = true;
        public bool listenWarning = true;
        public bool listenError = true;

        private static Logger instance;

        public static Logger GetInstance()
        {
            if (null == instance)
            {
                if (Application.isPlaying)
                {
                    GameObject obj = new GameObject("Logger");

                    DontDestroyOnLoad(obj);
                    instance = obj.AddComponent<Logger>();
                }
                else
                {
                    return null;
                }
            }
            return instance;
        }

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);

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
            Message(message);
        }
        private void OnWarningMessage(string message)
        {
            Debug.LogWarning(message);
        }
        private void OnErrorMessage(string message)
        {
            Debug.LogError(message);
        }

        public void Message(string message)
        {
            Debug.Log(message);
        }
        public void Warning(string message)
        {
            Debug.LogWarning(message);
        }
        public void Error(string message)
        {
            Debug.LogError(message);
        }
    }
}