using UnityEngine;

namespace Cam
{
    [RequireComponent(typeof(Camera))]
    public class CameraObliqueFrustum : MonoBehaviour
    {
        [SerializeField]
        [Range(-1.0f, 1.0f)]
        private float horizontalOblique;
        [SerializeField]
        [Range(-1.0f, 1.0f)]
        private float verticalOblique;

        private void Start()
        {
            SetObliqueness(horizontalOblique, verticalOblique);
        }
        //private void Update()
        //{
        //    SetObliqueness(horizontalOblique, verticalOblique);
        //}
        void SetObliqueness(float horizObl, float vertObl)
        {
            Camera camera = GetComponent<Camera>();
            Matrix4x4 mat = camera.projectionMatrix;
            mat[0, 2] = horizObl;
            mat[1, 2] = vertObl;
            camera.projectionMatrix = mat;
        }
    }
}