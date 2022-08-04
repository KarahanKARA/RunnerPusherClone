using TMPro;
using UnityEngine;

namespace Player
{
    public class CrowdCollisions : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("MathObjTag"))
            {
                string mathObjText = collision.gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
                GameManager.Instance.OnCollisionWithMathObj(mathObjText);
                Destroy(collision.gameObject);
            }

            if (collision.collider.CompareTag("PushingStageTag"))
            {
                StartCoroutine(GameManager.Instance.ApproachThem(gameObject,
                    collision.gameObject.transform.parent.gameObject));
                Destroy(collision.collider.gameObject);
                var pos = transform.position;
                var lanePos = collision.collider.gameObject.transform.position;
                pos = new Vector3(lanePos.x,0,pos.z);
                transform.position = pos;
                GameManager.Instance.CanPlayerSwipe = false;
                GameManager.Instance.CanPlayerMoveForward = false;
                GameManager.Instance.CanCameraMove = false;
            }

            if (collision.collider.CompareTag("EndgameTag"))
            {
                Destroy(collision.collider.gameObject);
                GameManager.Instance.IsPlayerOnEndgame = true;
                GameManager.Instance.CanPlayerSwipe = false;
            }
        }
    }
}