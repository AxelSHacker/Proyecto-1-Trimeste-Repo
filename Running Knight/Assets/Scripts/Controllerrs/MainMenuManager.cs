using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string _tutorialScene;
    void Start()
    {
        GameManager.Instance.AlphaCanvas(GameManager.Instance.endGameCanvasGroup, 0, false);
        GameManager.Instance.AlphaCanvas(GameManager.Instance.continueCanvasGroup, 0, false);
        GameManager.Instance.AlphaCanvas(GameManager.Instance.pauseCanvasGroup, 0, false);
        MusicManager.Instance.PlayMainMenuMusic();
    }
    public void ExitGGame()
    {
        Application.Quit();
    }
    public void OptionMenu()
    {
        GameManager.Instance.OptionMenu();
    }
    public void StartGame(string _sceneName)
    {
        SceneManager.Instance.LoadScene(_sceneName, true);
    }
    public void TutorialScene()
    {
        SceneManager.Instance.LoadScene(_tutorialScene, true);
    }

}
