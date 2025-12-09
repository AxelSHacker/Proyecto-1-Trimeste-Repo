using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Slider musicVolumen;
    [SerializeField] Slider sfxVolumen;

    void Start()
    {

        MusicManager.Instance.PlayMainMenuMusic();
        MusicManager.Instance.PitchRegular();
    }

    void Update()
    {
        MusicManager.Instance.audioSource.volume = musicVolumen.value;
        MusicManager.Instance.sfxSource.volume = sfxVolumen.value;
    }
    public void ChangScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGGame()
    {
        Application.Quit();
    }


}
