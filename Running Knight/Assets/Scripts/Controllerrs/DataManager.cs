using UnityEngine;

public class DataManager : MonoBehaviour
{
    //Puntuacion maxima registrada
    public int maxScore = 0;
    public int tutorialScore = 0;
    public int actualGameScore = 0;
    private static DataManager _instance;
    public static DataManager Instance => _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Load();
        }
        else
        {
            Destroy(this);
        }
    }
    /// <summary>
    /// guardado de datos n los playeperfs
    public void Save()
    {
        PlayerPrefs.SetInt("maxScore", maxScore);
    }
    /// <summary>
    /// Carga de datos desde los playerprefs.
    /// </summary>
    public void Load()
    {
        if (!PlayerPrefs.HasKey("maxScore")) return;
        maxScore = PlayerPrefs.GetInt("maxScore");
    }
    /// <summary>
    /// Lmpia toda la informacion guardada en los player
    /// </summary>
    public void CleaData()
    {
        PlayerPrefs.DeleteAll();
    }
    public void ClearMaxScore()
    {
        PlayerPrefs.DeleteKey("maxScore");
    }
    public void RestartGame()
    {
        actualGameScore = 0;
        Debug.Log("Entro aqui");
    }
 
}
