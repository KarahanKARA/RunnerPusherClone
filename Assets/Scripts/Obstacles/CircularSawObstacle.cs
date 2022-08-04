using System.Collections;
using UnityEngine;

namespace Obstacles
{
    public class CircularSawObstacle : MonoBehaviour
    {
        [SerializeField] private RectTransform lineTransform;
        [SerializeField] private GameObject circularSawModel;
        [SerializeField] private float rotateSpeed;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float movingDistance;

        private float _minLimit;
        private float _maxLimit;

        private void Start()
        {
            var pos = circularSawModel.transform.position;
            _minLimit = pos.x - movingDistance / 2f;
            _maxLimit = pos.x + movingDistance / 2f;
            lineTransform.localScale = new Vector3(movingDistance+3.5f,1,1);
            StartCoroutine(DecreasePositionValue());
        }

        void Update()
        {
            circularSawModel.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }


        private IEnumerator DecreasePositionValue()
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                var pos = circularSawModel.transform.position;
                pos.x -= Time.deltaTime * moveSpeed;
                if (pos.x < _minLimit)
                {
                    break;
                }

                circularSawModel.transform.position = pos;
            }

            StartCoroutine(IncreasePositionValue());
        }

        private IEnumerator IncreasePositionValue()
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                var pos = circularSawModel.transform.position;
                pos.x += Time.deltaTime * moveSpeed;
                if (pos.x > _maxLimit)
                {
                    break;
                }

                circularSawModel.transform.position = pos;
            }

            StartCoroutine(DecreasePositionValue());
        }
    }
}