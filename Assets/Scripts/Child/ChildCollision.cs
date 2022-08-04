using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Child
{
    public class ChildCollision : MonoBehaviour
    {
        private Material _onCollisionMaterial;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ColorShiftTag"))
            {
                _onCollisionMaterial = new Material(Shader.Find("Standard"));
                Color color = other.gameObject.GetComponentInChildren<Image>().color;
                color.a = 255;
                _onCollisionMaterial.color = color;
                GetComponentInChildren<SkinnedMeshRenderer>().material = _onCollisionMaterial;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Brick"))
            {
                GameManager.Instance.LockTheFormation = true;
                transform.parent = null;
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
