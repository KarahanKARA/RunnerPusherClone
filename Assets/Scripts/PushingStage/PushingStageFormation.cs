using TMPro;
using UnityEngine;

namespace PushingStage
{
    public class PushingStageFormation : MonoBehaviour
    {
        [SerializeField] private GameObject enemyMousesParent;
        [SerializeField] private TextMeshProUGUI mouseCountText;
        [SerializeField] private int formationVerticalDistance;
        [SerializeField] private int formationHorizontalDistance;
        
        private void Start()
        {
            int temp = 0;
            mouseCountText.text = enemyMousesParent.transform.childCount.ToString();
            foreach (Transform child in enemyMousesParent.transform)
            {
                var targetPos = transform.position;
                targetPos.z += formationVerticalDistance * (((temp + 3) / 3)-1);
                if (temp % 3 == 2)
                {
                    targetPos.x -= formationHorizontalDistance;
                }
                else if (temp % 3 == 1)
                {
                    targetPos.x += formationHorizontalDistance;
                }
                child.position = targetPos;
                temp++;
            }
        }
    }
}