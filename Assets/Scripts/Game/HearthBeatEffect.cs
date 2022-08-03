using System.Collections;
using UnityEngine;

namespace Game
{
    public class HearthBeatEffect : MonoBehaviour
    {
        private float _onStartScaleX;
        private float _scaleX, _scaleY, _scaleZ;

        private void OnEnable()
        {
            var localScale = transform.localScale;
            _scaleX = localScale.x;
            _scaleY = localScale.y;
            _scaleZ = localScale.z;
            _onStartScaleX = _scaleX;
            StartCoroutine(Shrink());
        }

        private IEnumerator Shrink()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                _scaleX -= 0.08f;
                _scaleY -= 0.08f;
                _scaleZ -= 0.08f;
                transform.localScale = new Vector3(_scaleX, _scaleY, _scaleZ);
                if (_scaleX <= _onStartScaleX*0.8f)
                {
                    StartCoroutine(Enlarge());
                    break;
                }
            }
        }
        private IEnumerator Enlarge()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                _scaleX += 0.08f;
                _scaleY += 0.08f;
                _scaleZ += 0.08f;

                transform.localScale = new Vector3(_scaleX, _scaleY, _scaleZ);
                if (_scaleX >= _onStartScaleX)
                {
                    StartCoroutine(Shrink());
                    break;
                }
            }
        }
    }
}
