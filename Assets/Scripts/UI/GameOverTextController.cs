using UnityEngine;

namespace UI
{
    public class GameOverTextController : MonoBehaviour
    {
        [SerializeField]
        private GameObject loseText;
        [SerializeField]
        private GameObject winText;

        public void ShowLoseText()
        {
            loseText.SetActive(true);
        }

        public void ShowWinText()
        {
            winText.SetActive(true);
        }
    }
}