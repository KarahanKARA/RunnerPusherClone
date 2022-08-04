using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class FormationSystem : MonoBehaviour
    {
        public GameObject targetObject;
        [SerializeField] private GameObject mousesParent;
        [SerializeField] private int formationVerticalDistance;
        [SerializeField] private int formationHorizontalDistance;
        private float speed = 10;
        private List<Vector3> pathList = new List<Vector3>();

        void Update()
        {
            var targetPos = targetObject.transform.position;
            pathList.Insert(0, targetPos);
            int temp = 0;
            foreach (Transform child in mousesParent.transform)
            {
                var point = pathList[Mathf.Min((temp) * formationVerticalDistance, pathList.Count - 1)];
                
                point.z = targetPos.z - formationVerticalDistance * (((temp + 3) / 3)-1);
                if (temp % 3 == 2)
                {
                    point.x -= formationHorizontalDistance;
                }
                else if (temp % 3 == 1)
                {
                    point.x += formationHorizontalDistance;
                }
                Vector3 moveDirection = point - child.transform.position;
                moveDirection.y = 0;
                child.position += moveDirection * speed * Time.deltaTime;
                temp++;
            }
        }
    }
}