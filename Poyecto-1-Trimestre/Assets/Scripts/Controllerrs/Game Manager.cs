using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    #region variables
    public int collectableCount;



    public TextMeshProUGUI pointTMP;
    [Header("HUD")]
    public CanvasGroup canvasGroup;

    [Header("End Game Panel")]
    public CanvasGroup endGameCanvasGroup;

    public TextMeshProUGUI finalScoreTMP;

    public TextMeshProUGUI maxScoerTMP;
    public ParticleSystem nuke;

    [Header("Pause Menu")]
    public CanvasGroup pauseCanvasGroup;
    private static GameManager _instance;

    public static GameManager Instance => _instance;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

    }

    void Start()
    {
        pointTMP.text = collectableCount.ToString();
        endGameCanvasGroup.SetEnable(false);
        pauseCanvasGroup.SetEnable(false);
        maxScoerTMP.text = DataManager.Instance.maxScore.ToString();
        Time.timeScale = 1;

    }

    // Update is called once per frame
    void Update()
    {

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
            maxScoerTMP.text = collectableCount.ToString();

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

    public void Invincibility(GameObject gameObject)
    {
        Destroy(gameObject);
    }




    public void ExitToMenu()
    {
        SceneManager.LoadScene("Main Menu");

    }






    #endregion

}
