using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Utils
{
    public class SceneSwitcher : MonoBehaviour
    {
        public float delay; //sec
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip clip;

        public void SwitchScene(string sceneName)
        {
            audioSource.PlayOneShot(clip);
            StartCoroutine(WaitAndSwitchScene(sceneName, delay));
        }

        private IEnumerator WaitAndSwitchScene(string sceneName, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            SceneManager.LoadScene(sceneName);
        }
    } 
}