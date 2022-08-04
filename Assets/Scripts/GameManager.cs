using System;
using System.Collections;
using PushingStage;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TODO: Sondan öldürmeye başla
    public static GameManager Instance;

    [SerializeField] private GameObject mousesParent;
    [SerializeField] private GameObject mousePrefab;
    [SerializeField] private float crowdApproachSpeed;

    private bool _isPushingSystemActive = false;

    public bool IsPushingSystemActive
    {
        get { return _isPushingSystemActive; }
        set { _isPushingSystemActive = value; }
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

        CrowdMembersCount = mousesParent.transform.childCount;
    }

    private void Update()
    {
        if (IsPushingSystemActive)
        {
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
            for (int i = 0; i < enemyCount; i++)
            {
                Instantiate(mousePrefab, mousesParent.transform.position, Quaternion.identity, mousesParent.transform);
            }
            CrowdMembersCount += enemyCount;
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
            //GAME OVER
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
            for (int i = 0; i < count; i++)
            {
                Instantiate(mousePrefab, mousesParent.transform.position, Quaternion.identity, mousesParent.transform);
            }

            CrowdMembersCount += count;
        }
        else if (additive[0] == '-')
        {
            x = stringAdditive.Split('-');
            var count = int.Parse(x[1]);
            if (count >= CrowdMembersCount)
            {
                Debug.Log("GAME OVER");
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    if (mousesParent.transform.childCount != 0)
                    {
                        Destroy(mousesParent.transform.GetChild(i).gameObject);
                    }
                }

                CrowdMembersCount -= count;
            }
        }
        else if (additive[0] == 'x')
        {
            x = stringAdditive.Split('x');
            var count = int.Parse(x[1]);
            count *= CrowdMembersCount;
            for (int i = 0; i < count - CrowdMembersCount; i++)
            {
                Instantiate(mousePrefab, mousesParent.transform.position, Quaternion.identity, mousesParent.transform);
            }

            CrowdMembersCount += count;
        }
        else if (additive[0] == '/')
        {
            x = stringAdditive.Split('/');
            var count = int.Parse(x[1]);
            if (CrowdMembersCount / count < 1)
            {
                Debug.Log("GAME OVER");
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

                CrowdMembersCount /= count;
            }
        }
    }
}