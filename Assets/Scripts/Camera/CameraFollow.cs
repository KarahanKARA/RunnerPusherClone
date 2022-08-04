using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed;
        private Vector3 _offset;

        void Start()
        {
            _offset = transform.position - target.position;
        }

        void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(target.position.x, 1, target.position.z) + _offset, Time.deltaTime * speed);
        }
    }
}