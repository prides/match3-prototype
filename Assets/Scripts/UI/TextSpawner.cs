using UnityEngine;

namespace UI
{
    public class TextSpawner : MonoBehaviour
    {
        private static TextSpawner instance;
        public GameObject textPrefab;

        public static TextSpawner GetInstance()
        {
            if (null == instance)
            {
                if (Application.isPlaying)
                {
                    GameObject obj = new GameObject("TextSpawner");
                    DontDestroyOnLoad(obj);
                    instance = obj.AddComponent<TextSpawner>();
                }
                else
                {
                    return null;
                }
            }
            return instance;
        }

        public void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            textPrefab = (GameObject)Resources.Load("Prefabs/TextMessagePrefab");
        }

        public void SpawnTextMessage(string message, Vector3 position, Color color)
        {
            GameObject text = Instantiate(textPrefab, position, Quaternion.identity);
            TextMesh textMesh = text.GetComponentInChildren<TextMesh>();
            textMesh.text = message;
            textMesh.color = color;
        }
    } 
}
