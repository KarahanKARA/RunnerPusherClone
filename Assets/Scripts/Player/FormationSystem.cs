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
        [SerializeField] private float formationHeightDistance;

        private float speed = 10;
        private List<Vector3> pathList = new List<Vector3>();
        private static readonly int Idle = Animator.StringToHash("Idle");

        void Update()
        {
            if (!GameManager.Instance.IsPlayerOnEndgame)
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
            else
            {
                if (!GameManager.Instance.LockTheFormation)
                {
                    var pos = transform.position;
                    pos.x = 0;
                    transform.position = pos;
                
                    int temp = 0;
                    foreach (Transform child in mousesParent.transform)
                    {
                        child.GetComponent<CapsuleCollider>().isTrigger = false;
                        child.gameObject.GetComponent<Rigidbody>().useGravity = false;
                        child.gameObject.GetComponent<Rigidbody>().constraints = 
                            RigidbodyConstraints.FreezeRotationX |
                            RigidbodyConstraints.FreezeRotationY |
                            RigidbodyConstraints.FreezeRotationZ;
                        var targetPos = transform.position;
                        targetPos.y += formationHeightDistance * (((temp + 3) / 3)-1);
                        if (temp % 3 == 2)
                        {
                            targetPos.x -= formationHorizontalDistance;
                        }
                        else if (temp % 3 == 1)
                        {
                            targetPos.x += formationHorizontalDistance;
                        }

                        var dir = targetPos-child.position;
                        child.position += dir * (speed * Time.deltaTime/2f);
                        temp++;
                        child.GetComponentInChildren<Animator>().SetBool(Idle,true);
                    }
                }
                else
                {
                    var pos = transform.position;
                    pos.x = 0;
                    transform.position = pos;
                
                    foreach (Transform child in mousesParent.transform)
                    {
                        var childPos = child.position;
                        childPos.z += speed * Time.deltaTime;
                        child.position = childPos;
                    }

                    if (mousesParent.transform.childCount == 0)
                    {
                        GameManager.Instance.CanCameraMove = false;
                    }
                }
            }
        }
    }
}