using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
   public void ChangScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGGame()
    {
        Application.Quit();
    }
}
