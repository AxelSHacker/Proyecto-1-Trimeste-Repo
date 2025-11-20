using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    #region variables
    private int collectableCount;

    public TextMeshProUGUI pointTMP;
    [Header("HUD")]
    public CanvasGroup canvasGroup;

    [Header("End Game Panel")]
    public CanvasGroup endGameCanvasGroup;

    public TextMeshProUGUI finalScoreTMP;
    private static GameManager _instance;

    public static GameManager Instance => _instance;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
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
        canvasGroup.SetEnable(false);
        endGameCanvasGroup.SetEnable(true);
        finalScoreTMP.text = collectableCount.ToString();
        

    }
    
    // Reinicia la partida
    
    public void Restart()
    {
        //Recargamos la scena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}
