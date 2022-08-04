using System.Collections;
using UnityEngine;

namespace Obstacles
{
    public class BoxingObstacle : MonoBehaviour
    {
        [SerializeField] private GameObject movingObject;
        [SerializeField] private float speed;
        [SerializeField] private float movingDistance;
        [SerializeField] private float attackTime;
        [SerializeField] private Enums.BoxingObstaclePosition donatPosition;
        private float _minLimit;
        private float _maxLimit;
        private Vector3 _onStartPos;
        private void Awake()
        {
            var pos = movingObject.transform.position;
            _onStartPos = pos;
            _minLimit = pos.x - movingDistance / 2f;
            _maxLimit = pos.x + movingDistance / 2f;
            StartCoroutine(Waiter());
        }


        private IEnumerator Waiter()
        {
            while (true)
            {
                yield return new WaitForSeconds(attackTime);
                yield return StartCoroutine(DecreasePositionValue());
            }
        }
        private IEnumerator DecreasePositionValue()
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                var pos = movingObject.transform.position;

                if (donatPosition == Enums.BoxingObstaclePosition.LeftSide)
                {
                    pos.x += Time.deltaTime * speed;
                    if (pos.x > _maxLimit)
                    {
                        break;
                    }
                }
                else if (donatPosition == Enums.BoxingObstaclePosition.RightSide)
                {
                    pos.x -= Time.deltaTime * speed;
                    if (pos.x < _minLimit)
                    {
                        break;
                    }
                }
                movingObject.transform.position = pos;
            }
            yield return StartCoroutine(IncreasePositionValue());
            movingObject.transform.position = _onStartPos;
        }

        private IEnumerator IncreasePositionValue()
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                var pos = movingObject.transform.position;

                if (donatPosition == Enums.BoxingObstaclePosition.LeftSide)
                {
                    pos.x -= Time.deltaTime * speed;
                    if (pos.x < _onStartPos.x)
                    {
                        break;
                    }
                }
                else if (donatPosition == Enums.BoxingObstaclePosition.RightSide)
                {
                    pos.x += Time.deltaTime * speed;
                    if (pos.x > _onStartPos.x)
                    {
                        break;
                    }
                }
                movingObject.transform.position = pos;
            }
        }
    }
}
