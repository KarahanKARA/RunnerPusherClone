using System.Collections;
using UnityEngine;

namespace Obstacles
{
    public class RotatingObstacle : MonoBehaviour
    {
        [SerializeField] private GameObject rotatingObject;
        [SerializeField] private float speed;

        private float _zAxisValue;

        private void Start()
        {
            StartCoroutine(DecreaseZRotation());
        }

        private IEnumerator DecreaseZRotation()
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);

                if (_zAxisValue <= -90)
                {
                    break;
                }

                _zAxisValue -= Time.deltaTime * speed;
                rotatingObject.transform.localRotation = Quaternion.Euler(0,0,_zAxisValue);
            }

            StartCoroutine(IncreaseZRotation());
        }
        private IEnumerator IncreaseZRotation()
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                if (_zAxisValue >= 90)
                {
                    break;
                }

                _zAxisValue += Time.deltaTime * speed;
                rotatingObject.transform.rotation = Quaternion.Euler(0,0,_zAxisValue);
            }

            StartCoroutine(DecreaseZRotation());
        }
    }
}
