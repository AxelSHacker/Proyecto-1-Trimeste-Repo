using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject optionMenu;
    [SerializeField] bool _transicionando;

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
