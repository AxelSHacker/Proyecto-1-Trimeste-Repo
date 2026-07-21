
using System;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class GameManager : MonoBehaviour
{
    #region variables
    public int collectableCount = 1;
    public TextMeshProUGUI pointTMP;
    [Header("HUD")]
    public CanvasGroup canvasGroup;

    [Header("Continue Panel")]
    public CanvasGroup continueCanvasGroup;
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

    public GameObject cheatsMenuPanel;
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
        AlphaCanvas(continueCanvasGroup, 0, false);
        optionMenu.SetActive(false);
        AlphaCanvas(endGameCanvasGroup, 0, false);
        AlphaCanvas(pauseCanvasGroup, 0, false);
        maxScoerTMP.text = DataManager.Instance.maxScore.ToString();
        collectableCount = DataManager.Instance.actualGameScore;
        pointTMP.text = collectableCount.ToString();

        Time.timeScale = 1;
    }
    #region Methos
    public void PicupCollectable(int value)
    {
        collectableCount += value;
        pointTMP.text = collectableCount.ToString();
        DataManager.Instance.actualGameScore = collectableCount;
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
            AlphaCanvas(continueCanvasGroup, 1, true);
            continueScoreTMP.text = collectableCount.ToString();
        }
        else
        {
            //Actuaiamos e texto que muestrta el rercord
            AlphaCanvas(continueCanvasGroup, 1, true);
            continueScoreTMP.text = collectableCount.ToString();

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
            AlphaCanvas(canvasGroup, 0, false);
            AlphaCanvas(endGameCanvasGroup, 1, true);
            finalScoreTMP.text = collectableCount.ToString();
        }
        else
        {
            //Actuaiamos e texto que muestrta el rercord
            maxScoerTMP.text = DataManager.Instance.maxScore.ToString();

            AlphaCanvas(canvasGroup, 0, false);
            AlphaCanvas(endGameCanvasGroup, 1, true);
            finalScoreTMP.text = collectableCount.ToString();
        }

    }
    public void ContinueButton()
    {
        AlphaCanvas(continueCanvasGroup, 0, false);
        SceneManager.Instance.LoadScene("Platform 2D", true);
    }
    // Reinicia la partida
    public void Restart()
    {
        //Recargamos la scena actual
        AlphaCanvas(endGameCanvasGroup, 0, false);
        DataManager.Instance.actualGameScore = 0;
        SceneManager.Instance.LoadScene("EndLessRuner", true);
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
        Time.timeScale = 1f;
        SceneManager.Instance.LoadScene("Main Menu", true);
    }
    public void OptionMenu()
    {
        optionMenu.SetActive(!optionMenu.activeSelf);
    }
    public void ToggleCheatsMenu()
    {
        cheatsMenuPanel.SetActive(!cheatsMenuPanel.activeSelf);
    }
    public void ChangeTimeScale(float value)
    {
        value = Mathf.Clamp01(value);
        Time.timeScale = value;
    }
    public void AlphaCanvas(CanvasGroup canvasGroup, int alpha, bool activate)
    {
        canvasGroup.alpha = alpha;
        canvasGroup.blocksRaycasts = activate;
        canvasGroup.interactable = activate;
    }
    #endregion

}
