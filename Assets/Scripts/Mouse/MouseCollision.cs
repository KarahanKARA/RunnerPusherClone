using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Mouse
{
    public class MouseCollision : MonoBehaviour
    {
        private Material _onCollisionMaterial;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ColorShiftTag"))
            {
                Debug.Log(other.gameObject.name);
                _onCollisionMaterial = new Material(Shader.Find("Standard"));
                Color color = other.gameObject.GetComponentInChildren<Image>().color;
                color.a = 255;
                _onCollisionMaterial.color = color;
                GetComponentInChildren<SkinnedMeshRenderer>().material = _onCollisionMaterial;
            }
        }
    }
}
