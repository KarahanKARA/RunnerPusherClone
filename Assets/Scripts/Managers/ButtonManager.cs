using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class ButtonManager : MonoBehaviour
    {
        public void NextLevelButtonOnClick()
        {
            int index = SceneManager.GetActiveScene().buildIndex == 3 ? 1 : GameManager.Instance.SceneIndex + 1;
            SceneManager.LoadScene(index);
        }
        public void RetryButtonOnClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void MenuButtonOnClick()
        {
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }
        public void StartButtonOnClick()
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
        public void ExitButtonOnClick()
        {
            Application.Quit();
        }
    }
}
