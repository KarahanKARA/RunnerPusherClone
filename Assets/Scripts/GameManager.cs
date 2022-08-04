using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TODO: Sondan öldürmeye başla
    public static GameManager Instance;

    [SerializeField] private GameObject mousesParent;
    [SerializeField] private GameObject mousePrefab;

    private int _crowdMembersCount;

    public int CrowdMembersCount
    {
        get { return _crowdMembersCount; }
        set { _crowdMembersCount = value; }
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
            for (int i = 0; i < count-CrowdMembersCount; i++)
            {
                Instantiate(mousePrefab, mousesParent.transform.position, Quaternion.identity, mousesParent.transform);
            }

            CrowdMembersCount += count;
        }
        else if (additive[0] == '/')
        {
            x = stringAdditive.Split('/');
            var count = int.Parse(x[1]);
            if (CrowdMembersCount / count<1)
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