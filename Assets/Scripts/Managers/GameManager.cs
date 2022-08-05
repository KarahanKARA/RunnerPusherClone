using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [SerializeField] private TextMeshProUGUI childCountText;
        [SerializeField] private GameObject mousesParent;
        [SerializeField] private GameObject mousePrefab;
        [SerializeField] private Material mouseMaterial;
        [SerializeField] private float crowdApproachSpeed;

        [SerializeField] private GameObject gameOverButton;
        [SerializeField] private GameObject nextLevelButton;


        private int _sceneIndex;

        public int SceneIndex
        {
            get { return _sceneIndex; }
            set { _sceneIndex = value; }
        }

        private Color _currentColor;

        public Color CurrentColor
        {
            get { return _currentColor; }
            set { _currentColor = value; }
        }

        private bool _lockTheFormation = false;

        public bool LockTheFormation
        {
            get { return _lockTheFormation; }
            set { _lockTheFormation = value; }
        }

        private bool _isPlayerOnEndgame = false;

        public bool IsPlayerOnEndgame
        {
            get { return _isPlayerOnEndgame; }
            set { _isPlayerOnEndgame = value; }
        }

        private bool _canPlayerMoveForward = true;

        public bool CanPlayerMoveForward
        {
            get { return _canPlayerMoveForward; }
            set { _canPlayerMoveForward = value; }
        }

        private bool _canPlayerSwipe = true;

        public bool CanPlayerSwipe
        {
            get { return _canPlayerSwipe; }
            set { _canPlayerSwipe = value; }
        }


        private int _crowdMembersCount;

        public int CrowdMembersCount
        {
            get { return _crowdMembersCount; }
            set { _crowdMembersCount = value; }
        }

        private bool _canCameraMove = true;

        public bool CanCameraMove
        {
            get { return _canCameraMove; }
            set { _canCameraMove = value; }
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            _sceneIndex = SceneManager.GetActiveScene().buildIndex;
            CurrentColor = mouseMaterial.color;
            CrowdMembersCount = mousesParent.transform.childCount;
        }

        private void Update()
        {
            childCountText.text = CrowdMembersCount.ToString();
            if (CrowdMembersCount <= 0)
            {
                GameOver();
            }

            if (IsPlayerOnEndgame && (mousesParent.transform.childCount == 0))
            {
                nextLevelButton.SetActive(true);
            }

            if (!IsPlayerOnEndgame)
            {
                CrowdMembersCount = mousesParent.transform.childCount;
            }
        }

        public IEnumerator ApproachThem(GameObject crowd, GameObject stage)
        {
            float time = 0;
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                crowd.transform.position += new Vector3(0, 0, 1) * (Time.deltaTime * crowdApproachSpeed);
                if (Vector3.Distance(crowd.transform.position, stage.transform.position) <= 1.5f)
                {
                    break;
                }
            }

            var enemyCount = stage.transform.GetChild(0).childCount;
            while (time < 1.1f)
            {
                if (CrowdMembersCount > enemyCount)
                {
                    crowd.transform.position += new Vector3(0, 0, 1) * (Time.deltaTime * crowdApproachSpeed);
                    stage.transform.GetChild(0).transform.position +=
                        new Vector3(0, 0, 1) * (Time.deltaTime * crowdApproachSpeed);
                }
                else
                {
                    crowd.transform.position -= new Vector3(0, 0, 1) * (Time.deltaTime * crowdApproachSpeed);
                    stage.transform.GetChild(0).transform.position -=
                        new Vector3(0, 0, 1) * (Time.deltaTime * crowdApproachSpeed);
                }

                yield return new WaitForSeconds(.01f);
                time += Time.deltaTime;
            }

            if (CrowdMembersCount > enemyCount)
            {
                while (time < 1.1f)
                {
                    crowd.transform.position += new Vector3(0, 0, 1) * (Time.deltaTime * crowdApproachSpeed);
                    stage.transform.GetChild(0).transform.position +=
                        new Vector3(0, 0, 1) * (Time.deltaTime * crowdApproachSpeed);

                    yield return new WaitForSeconds(.01f);
                    time += Time.deltaTime;
                }

                Material temp = new Material(Shader.Find("Standard"));
                temp.color = CurrentColor;
                for (int i = 0; i < enemyCount; i++)
                {
                    var createdObj = Instantiate(mousePrefab, mousesParent.transform.position, Quaternion.identity,
                        mousesParent.transform);
                    createdObj.GetComponentInChildren<SkinnedMeshRenderer>().material = temp;
                }

                Destroy(stage);
                CanCameraMove = true;
                CanPlayerSwipe = true;
                CanPlayerMoveForward = true;
            }
            else
            {
                while (time < 1.5f)
                {
                    crowd.transform.position -= new Vector3(0, 0, 1) * (Time.deltaTime * crowdApproachSpeed * 1.5f);
                    stage.transform.GetChild(0).transform.position -=
                        new Vector3(0, 0, 1) * (Time.deltaTime * crowdApproachSpeed * 1.5f);
                    yield return new WaitForSeconds(.01f);
                    time += Time.deltaTime;
                }

                GameOver();
            }
        }

        public void OnCollisionWithMathObj(string objText)
        {
            var additive = objText.ToCharArray();
            string stringAdditive = new string(additive);
            string[] x;
            if (additive[0] == '+')
            {
                x = stringAdditive.Split('+');
                var count = int.Parse(x[1]);
                Material temp = new Material(Shader.Find("Standard"));
                temp.color = CurrentColor;
                for (int i = 0; i < count; i++)
                {
                    var createdObj = Instantiate(mousePrefab, mousesParent.transform.position, Quaternion.identity,
                        mousesParent.transform);
                    createdObj.GetComponentInChildren<SkinnedMeshRenderer>().material = temp;
                }

            }
            else if (additive[0] == '-')
            {
                x = stringAdditive.Split('-');
                var count = int.Parse(x[1]);
                if (count >= CrowdMembersCount)
                {
                    GameOver();
                }
                else
                {
                    for (int i = CrowdMembersCount - 1; i > CrowdMembersCount - count - 1; i--)
                    {
                        if (mousesParent.transform.childCount != 0)
                        {
                            Destroy(mousesParent.transform.GetChild(i).gameObject);
                        }
                    }
                }
            }
            else if (additive[0] == 'x')
            {
                x = stringAdditive.Split('x');
                var count = int.Parse(x[1]);
                count *= CrowdMembersCount;
                Material temp = new Material(Shader.Find("Standard"));
                temp.color = CurrentColor;
                for (int i = 0; i < count - CrowdMembersCount; i++)
                {
                    var createdObj = Instantiate(mousePrefab, mousesParent.transform.position, Quaternion.identity,
                        mousesParent.transform);
                    createdObj.GetComponentInChildren<SkinnedMeshRenderer>().material = temp;
                }

            }
            else if (additive[0] == '/')
            {
                x = stringAdditive.Split('/');
                var count = int.Parse(x[1]);
                if (CrowdMembersCount / count < 1)
                {
                    GameOver();
                }
                else
                {
                    for (int i = 0; i < CrowdMembersCount - (CrowdMembersCount / count); i++)
                    {
                        if (mousesParent.transform.childCount != 0)
                        {
                            Destroy(mousesParent.transform.GetChild(i).gameObject);
                        }
                    }
                }
            }
        }

        private void GameOver()
        {
            gameOverButton.SetActive(true);
            CanPlayerSwipe = false;
            CanCameraMove = false;
            CanPlayerMoveForward = false;
        }
    }
}