using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SwipeCameraMover : MonoBehaviour
    {
        public Camera camera;

        private Vector3 lastMousePosition = Vector3.zero;

        [SerializeField]
        private float multiplier = 1.0f;
        private RangeAttribute cameraBound = new RangeAttribute(-3.0f, 3.0f);
        private Vector3 cameraTargetPosition = Vector3.zero;

        private void Start()
        {
            cameraTargetPosition = camera.transform.localPosition;
        }

        private void OnMouseDown()
        {
            lastMousePosition = Input.mousePosition;
        }

        private void OnMouseDrag()
        {
            cameraTargetPosition.x += (Input.mousePosition.x - lastMousePosition.x) * multiplier;
            if (cameraTargetPosition.x < cameraBound.min)
            {
                cameraTargetPosition.x = cameraBound.min;
            }
            if (cameraTargetPosition.x > cameraBound.max)
            {
                cameraTargetPosition.x = cameraBound.max;
            }
            lastMousePosition = Input.mousePosition;
        }

        private void Update()
        {
            if (cameraTargetPosition != camera.transform.localPosition)
            {
                Vector3 diff = cameraTargetPosition - camera.transform.localPosition;
                camera.transform.localPosition += diff * Time.deltaTime * 3.0f;
            }
        }
    } 
}