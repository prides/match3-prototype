using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Utils
{
    public class SceneSwitcher : MonoBehaviour
    {
        public float delay; //sec

        public void SwitchScene(string sceneName)
        {
            StartCoroutine(WaitAndSwitchScene(sceneName, delay));
        }

        private IEnumerator WaitAndSwitchScene(string sceneName, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            SceneManager.LoadScene(sceneName);
        }
    } 
}