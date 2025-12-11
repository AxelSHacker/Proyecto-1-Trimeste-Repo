using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Slider musicVolumen;
    [SerializeField] Slider sfxVolumen;

    [SerializeField]
    GameObject optionMenu;

    void Start()
    {
        MusicManager.Instance.PlayMainMenuMusic();

        
    }

       
        


    void Update()
    {
        MusicManager.Instance.audioSource.volume = musicVolumen.value;
        MusicManager.Instance.sfxSource.volume = sfxVolumen.value;

        DataManager.Instance.sfxVolumen = sfxVolumen.value;
        DataManager.Instance.musicVolumen = musicVolumen.value;

        MusicManager.Instance.audioSource.volume = DataManager.Instance.musicVolumen;
        
    }
    public void ChangScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGGame()
    {
        Application.Quit();
    }

    public void OptionMenu()
    {
        optionMenu.SetActive(!optionMenu.activeSelf);

    }

    void OnDisable()
    {
        DataManager.Instance.SaveVolumenParameters();
    }


}
