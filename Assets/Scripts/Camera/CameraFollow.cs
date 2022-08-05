using Managers;
using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed;
        private Vector3 _offset;
        private float _onStartCamSpeed;
        void Start()
        {
            _offset = transform.position - target.position;
            _onStartCamSpeed = speed;
        }

        void LateUpdate()
        {
            if (GameManager.Instance.CanCameraMove)
            {
                if (GameManager.Instance.IsPlayerOnEndgame)
                {
                    speed = _onStartCamSpeed / 10f;
                    var targetPos = target.transform.position;
                    targetPos.y += Time.deltaTime * _onStartCamSpeed;
                    target.transform.position = targetPos;
                    _offset.z -= Time.deltaTime * _onStartCamSpeed;
                }
                var movingPos = target.position + _offset;
                transform.position = Vector3.Lerp(transform.position,movingPos, Time.deltaTime * speed);
            }
        }
    }
}