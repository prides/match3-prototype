using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer progressRenderer = null;

        [SerializeField]
        private float progress = 0.0f;

        private void Start()
        {
            if (progressRenderer == null)
            {
                progressRenderer = GetComponent<MeshRenderer>();
            }
        }

        public void SetProgress(float progress)
        {
            this.progress = progress;
            RefreshProgress();
        }

        private void RefreshProgress()
        {
            progressRenderer.material.SetTextureOffset("_MainTex", new Vector2(1.0f - progress, 0.0f));
        }
    } 
}