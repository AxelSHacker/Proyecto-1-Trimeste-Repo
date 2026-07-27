
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region variables
    public int collectableCount;
    public TextMeshProUGUI pointTMP;
    [Header("HUD")]
    
    public CanvasGroup canvasGroup;
    [SerializeField] CanvasGroup[] canvasGroups;

    [Header("Continue Panel")]
    public CanvasGroup continueCanvasGroup;
    public TextMeshProUGUI continueScoreTMP;
    public TextMeshProUGUI maxScoerTMPcontinueplanel;
    public Button continueButton;

    public ParticleSystem nuke;
    [Header("End Game Panel")]
    public CanvasGroup endGameCanvasGroup;
    public TextMeshProUGUI finalScoreTMP;
    public TextMeshProUGUI maxScoerTMP;
    public Button restartButton;

    [Header("Pause Menu")]
    public CanvasGroup pauseCanvasGroup;
    [SerializeField] CanvasGroup optionMenu;
    bool isActive = false;
    public GameObject cheatsMenuPanel;

    [Header(""), SerializeField]
    AudioClip _pickUpColectable;
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
        AlphaCanvas(optionMenu, 0, false);
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
        MusicManager.Instance.SFXPlayer(_pickUpColectable);
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

            if (nuke == null) nuke = GameObject.FindWithTag("Nuke")?.GetComponent<ParticleSystem>();
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
    public void ContinueButton(string sceneName)
    {
        AlphaCanvas(continueCanvasGroup, 0, false);
        SceneManager.Instance.LoadScene(sceneName, true);
    }
    // Reinicia la partida
    public void Restart(string sceneName)
    {
        //Recargamos la scena actual
        AlphaCanvas(endGameCanvasGroup, 0, false);
        DataManager.Instance.actualGameScore = 0;
        SceneManager.Instance.LoadScene(sceneName, true);
    }
    /// <summary>
    /// Inicia o termina el estado de pausa
    /// </summary>
    /// /// <param name="value"></param>
    public void Pause(bool value)
    {
        // Si value es true (Pausa) -> timeScale = 0. Si es false -> timeScale = 1
        Time.timeScale = value ? 0.01f : 1f;

        // Si value es true -> alpha = 1 (Visible). Si es false -> alpha = 0 (Invisible)
        float targetAlpha = value ? 1f : 0f;

        AlphaCanvas(pauseCanvasGroup, targetAlpha, value);
    }
    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.Instance.LoadScene("Main Menu", true);
    }
    public void OptionMenu()
    {
        isActive = !isActive;
        // Si isOptionMenuOpen es true -> alpha vale 1. Si es false -> alpha vale 0.
        AlphaCanvas(optionMenu, isActive ? 1f : 0.01f, isActive);
    }
    public void AsignarListenerButton(string sceneName, Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            SceneManager.Instance.LoadScene(sceneName, true);
        });
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
    public void AlphaCanvas(CanvasGroup canvasGroup, float alpha, bool activate)
    {
        canvasGroup.alpha = alpha;
        canvasGroup.blocksRaycasts = activate;
        canvasGroup.interactable = activate;
    }
    public void DisableCanvasGroup()
    {
        foreach (CanvasGroup canvasGroup in canvasGroups)
        {
            AlphaCanvas(canvasGroup, 0, false);
        }
    }
    #endregion

}
