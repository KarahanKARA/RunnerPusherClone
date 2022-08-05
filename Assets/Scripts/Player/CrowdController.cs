using Managers;
using UnityEngine;

namespace Player
{
    public class CrowdController : MonoBehaviour
    {
        [SerializeField] private float forwardSpeed;
        [SerializeField] private float swipeSpeed;
        [SerializeField] private float mobileSwipeSpeed;
        [SerializeField] private float mapLimitValue;
        private Rigidbody _rigidbody;
        private bool _isDragging;
        private Vector3 _currentCursorPos;
        private Vector3 _oldCursorPos;
        private Touch _touch;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        void Update()
        {
            if (GameManager.Instance.CanPlayerSwipe)
            {
#if UNITY_64
                SwipeMovementPC();
#endif
#if UNITY_ANDROID
            SwipeMovementMobile();
#endif
            }
            else
            {
                _isDragging = false;
            }

            if (GameManager.Instance.CanPlayerMoveForward)
            {
                ForwardMovement();
            }
        }

        private void ForwardMovement()
        {
            var pos = _rigidbody.position;
            pos.z += Time.deltaTime * forwardSpeed;
            transform.position = pos;
        }
        private void SwipeMovementPC()
        {
            var pos = _rigidbody.position;
            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
                _currentCursorPos = Input.mousePosition;
                _oldCursorPos = _currentCursorPos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }

            if (_isDragging)
            {
                _currentCursorPos = Input.mousePosition;
                var movementMagnitude = ((_currentCursorPos - _oldCursorPos).magnitude * swipeSpeed) / 400f;
                if (_currentCursorPos.x < _oldCursorPos.x)
                {
                    movementMagnitude = -movementMagnitude;
                }

                pos.x += movementMagnitude;
                _oldCursorPos = _currentCursorPos;
            }

            pos.x = Mathf.Clamp(pos.x, -mapLimitValue, mapLimitValue);
            _rigidbody.MovePosition(pos);
        }
        private void SwipeMovementMobile()
        {
            if (Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);

                if (_touch.phase == TouchPhase.Moved)
                {
                    var pos = _rigidbody.position;
                    pos.x += _touch.deltaPosition.x * mobileSwipeSpeed*Time.deltaTime;
                    pos.x = Mathf.Clamp(pos.x, -mapLimitValue, mapLimitValue);
                    _rigidbody.MovePosition(pos);
                }
            }
        }
    }
}