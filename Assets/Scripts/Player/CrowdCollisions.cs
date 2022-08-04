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
        }
    }
}