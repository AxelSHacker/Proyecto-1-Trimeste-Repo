using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject optionMenu;
    [SerializeField] bool _transicionando;
    void Start()
    {
        GameManager.Instance.AlphaCanvas(GameManager.Instance.endGameCanvasGroup, 0, false);
        GameManager.Instance.AlphaCanvas(GameManager.Instance.continueCanvasGroup, 0, false);
        MusicManager.Instance.PlayMainMenuMusic();
    }
    public void ExitGGame()
    {
        Application.Quit();
    }

    public void OptionMenu()
    {
        optionMenu.SetActive(!optionMenu.activeSelf);

    }
    public void StartGame(string _sceneName )
    {
        SceneManager.Instance.LoadScene(_sceneName, true);
    }



}
