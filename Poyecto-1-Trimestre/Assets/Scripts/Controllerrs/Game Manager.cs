using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    #region variables
    public int collectableCount = 1;

    public TextMeshProUGUI pointTMP;
    [Header("HUD")]
    public CanvasGroup canvasGroup;

    [Header("Continue Panel")]
    public CanvasGroup continueCanvasGroup;
    public TextMeshProUGUI healtText;
    public TextMeshProUGUI continueScoreTMP;

    public TextMeshProUGUI maxScoerTMPcontinueplanel;


    public ParticleSystem nuke;
    [Header("End Game Panel")]
    public CanvasGroup endGameCanvasGroup;

    public TextMeshProUGUI finalScoreTMP;

    public TextMeshProUGUI maxScoerTMP;



    [Header("Pause Menu")]
    public CanvasGroup pauseCanvasGroup;
    [SerializeField] GameObject optionMenu;

    [Header("Transition")]

    public bool platformEndGame = false;

    private static GameManager _instance;

    public static GameManager Instance => _instance;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }


    }

    void Start()
    {
        MusicManager.Instance.PlayGameMusic();
        MusicManager.Instance.audioSource.volume = DataManager.Instance.musicVolumen;
        MusicManager.Instance.sfxSource.volume = DataManager.Instance.sfxVolumen;
        optionMenu.SetActive(false);
        pointTMP.text = collectableCount.ToString();
        continueCanvasGroup.SetEnable(false);
        endGameCanvasGroup.SetEnable(false);
        pauseCanvasGroup.SetEnable(false);
        maxScoerTMP.text = DataManager.Instance.maxScore.ToString();
        Time.timeScale = 1;

    }


    #region Methos

    public void PicupCollectable(int value)
    {
        collectableCount += value;
        pointTMP.text = collectableCount.ToString();
    }
    /// <summary>
    /// Gestiona las acciones del GameOver
    /// </summary>
    public void ContinueGame()
    {

        bool newRecord = DataManager.Instance.maxScore < collectableCount;
        //Si la puntuacion obttenida suora la maxima
        if (newRecord)
        {
            //Efecto
            nuke.Play();
            //Actualizamos el nuevo ecord
            DataManager.Instance.maxScore = collectableCount;
            //Guadamos 
            DataManager.Instance.Save();
            //Actuaiamos e texto que muestrta el rercord
            maxScoerTMPcontinueplanel.text = DataManager.Instance.maxScore.ToString();
            healtText.text = collectableCount.ToString();
            canvasGroup.SetEnable(false);
            continueCanvasGroup.SetEnable(true);
            continueScoreTMP.text = collectableCount.ToString();
        }
        else
        {
            //Actuaiamos e texto que muestrta el rercord
            canvasGroup.SetEnable(false);
            continueCanvasGroup.SetEnable(true);
            continueScoreTMP.text = collectableCount.ToString();
            healtText.text = collectableCount.ToString();
            maxScoerTMPcontinueplanel.text = DataManager.Instance.maxScore.ToString();
        }
    }

    public void EndGame()
    {

        bool newRecord = DataManager.Instance.maxScore < collectableCount;
        //Si la puntuacion obttenida suora la maxima
        if (newRecord)
        {
            //Efecto
            nuke.Play();
            //Actualizamos el nuevo ecord
            DataManager.Instance.maxScore = collectableCount;
            //Guadamos 
            DataManager.Instance.Save();
            //Actuaiamos e texto que muestrta el rercord
            maxScoerTMP.text = DataManager.Instance.maxScore.ToString();

            canvasGroup.SetEnable(false);
            endGameCanvasGroup.SetEnable(true);
            finalScoreTMP.text = collectableCount.ToString();
        }
        else
        {
            //Actuaiamos e texto que muestrta el rercord
            maxScoerTMP.text = DataManager.Instance.maxScore.ToString();

            canvasGroup.SetEnable(false);
            endGameCanvasGroup.SetEnable(true);
            finalScoreTMP.text = collectableCount.ToString();
        }
        
    }

    // Reinicia la partida
    public void Restart()
    {
        //Recargamos la scena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    /// <summary>
    /// Inicia o termina el estado de pausa
    /// </summary>
    /// <param name="value"></param>
    public void Pause(bool value)
    {
        //Segun el valor e value , asignamos una escala de tiempo diferente
        Time.timeScale = value ? 0f : 1f;
        //ACtivamos el canvas group del menu pausa
        pauseCanvasGroup.SetEnable(value);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Main Menu");

    }

    public void ContinueButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Invincibility(GameObject gameObject)
    {
        Destroy(gameObject);
    }


    public void OptionMenu()
    {
        optionMenu.SetActive(!optionMenu.activeSelf);

    }

    public void TransitionPlatformEndless()
    {
       
      
    }
       







    #endregion

}
