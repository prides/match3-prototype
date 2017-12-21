using UnityEngine;
using Match3Core;

using Match3Wrapper;

namespace PlayerInput
{
    public class SwipeComponent : MonoBehaviour
    {
        public delegate void SimpleSwipeComponentEvent(SwipeComponent sender, Direction direction);
        public event SimpleSwipeComponentEvent OnSwipeStartEvent;
        public event SimpleSwipeComponentEvent OnSwipeEndEvent;

        public enum State
        {
            Idle,
            Touched,
            Swiping
        }

        private Camera currentCamera;
        public Camera CurrentCamera
        {
            get
            {
                if (currentCamera == null)
                {
                    currentCamera = Camera.main;
                }
                return currentCamera;
            }
        }

        private Direction currentConstrains = Direction.None;
        public Direction CurrentConstrains
        {
            get { return currentConstrains; }
            set { currentConstrains = value; }
        }

        private float permitedDistance = 1.0f;
        public float PermitedDistance
        {
            get { return permitedDistance; }
            set { permitedDistance = value; }
        }

        [SerializeField]
        private float deadZoneDistance = 0.2f;

        [ReadOnly]
        [SerializeField]
        private Direction currentDirection = Direction.None;

        [SerializeField]
        private LayerMask layerMask = 0;

        private Vector3 basePosition = Vector3.zero;
        private Transform targetTransform;
        private GemControllerWrapper targetGemController;
        private State currentState = State.Idle;
        private Vector3 mouseDownWorldPosition = Vector3.zero;
        private bool isEnable = false;

        public void AddConstrain(Direction constrain)
        {
            currentConstrains |= constrain;
        }

        public void RemoveConstrain(Direction constrain)
        {
            currentConstrains = currentConstrains & ~constrain;
        }

        public void SetActive(bool value)
        {
            isEnable = value;
            gameObject.SetActive(value);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (currentState != State.Idle)
                {
                    SetToDefault();
                }
                mouseDownWorldPosition = CurrentCamera.ScreenToWorldPoint(Input.mousePosition);
                Collider2D overlappedCollider = Physics2D.OverlapPoint(mouseDownWorldPosition, layerMask);
                if (overlappedCollider != null)
                {
                    targetGemController = overlappedCollider.GetComponent<GemControllerWrapper>();
                    if (targetGemController.IsActive)
                    {
                        targetTransform = overlappedCollider.transform;
                        currentState = State.Touched;
                        currentConstrains = targetGemController.Constrains;
                        basePosition = new Vector3(targetGemController.CurrentX, targetGemController.CurrentY);
                    }
                }
                else
                {
                    mouseDownWorldPosition = Vector2.zero;
                    targetTransform = null;
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (currentState == State.Touched || currentState == State.Swiping)
                {
                    //OnDrag();
                    Vector3 mouseWorldPosition = CurrentCamera.ScreenToWorldPoint(Input.mousePosition);
                    float mouseDistance = Vector3.Distance(mouseDownWorldPosition, mouseWorldPosition);
                    Vector3 mouseDiff = mouseWorldPosition - mouseDownWorldPosition;
                    if (mouseDistance > deadZoneDistance && currentState == State.Touched)
                    {
                        if (Mathf.Abs(mouseDiff.x) > Mathf.Abs(mouseDiff.y))
                        {
                            if (mouseDiff.x > 0)
                            {
                                currentDirection = Direction.Right;
                            }
                            else
                            {
                                currentDirection = Direction.Left;
                            }
                        }
                        else
                        {
                            if (mouseDiff.y > 0)
                            {
                                currentDirection = Direction.Up;
                            }
                            else
                            {
                                currentDirection = Direction.Down;
                            }
                        }
                        targetGemController.OnSwipeEnd(this, currentDirection);
                        SetToDefault();
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                //if (currentState == State.Swiping)
                //{
                //    targetGemController.OnSwipeEnd(this, currentDirection);
                //}
                SetToDefault();
            }
        }

        private void OnDrag()
        {
            if (currentState != State.Touched && currentState != State.Swiping)
            {
                return;
            }
            Vector3 mouseWorldPosition = CurrentCamera.ScreenToWorldPoint(Input.mousePosition);
            float mouseDistance = Vector3.Distance(mouseDownWorldPosition, mouseWorldPosition);
            Vector3 mouseDiff = mouseWorldPosition - mouseDownWorldPosition;
            if (mouseDistance > deadZoneDistance && currentState == State.Touched)
            {
                currentState = State.Swiping;
                if (Mathf.Abs(mouseDiff.x) > Mathf.Abs(mouseDiff.y))
                {
                    if (mouseDiff.x > 0)
                    {
                        currentDirection = Direction.Right;
                    }
                    else
                    {
                        currentDirection = Direction.Left;
                    }
                }
                else
                {
                    if (mouseDiff.y > 0)
                    {
                        currentDirection = Direction.Up;
                    }
                    else
                    {
                        currentDirection = Direction.Down;
                    }
                }
                if ((currentConstrains & currentDirection) != Direction.None)
                {
                    SetToDefault();
                    return;
                }
                targetGemController.OnSwipeStart(this, currentDirection);
            }
            if (currentState == State.Swiping)
            {
                Vector3 position = targetTransform.position;
                switch (currentDirection)
                {
                    case Direction.Left:
                        position.x = mouseDiff.x <= -permitedDistance ? basePosition.x - permitedDistance : mouseDiff.x < 0.0f ? mouseWorldPosition.x : basePosition.x;
                        break;
                    case Direction.Right:
                        position.x = mouseDiff.x >= permitedDistance ? basePosition.x + permitedDistance : mouseDiff.x > 0.0f ? mouseWorldPosition.x : basePosition.x;
                        break;
                    case Direction.Up:
                        position.y = mouseDiff.y >= permitedDistance ? basePosition.y + permitedDistance : mouseDiff.y > 0.0f ? mouseWorldPosition.y : basePosition.y;
                        break;
                    case Direction.Down:
                        position.y = mouseDiff.y <= -permitedDistance ? basePosition.y - permitedDistance : mouseDiff.y < 0.0f ? mouseWorldPosition.y : basePosition.y;
                        break;
                }
                targetTransform.position = position;
            }
        }

        private void SetToDefault()
        {
            currentState = State.Idle;
            currentDirection = Direction.None;
            mouseDownWorldPosition = Vector3.zero;
        }
    } 
}