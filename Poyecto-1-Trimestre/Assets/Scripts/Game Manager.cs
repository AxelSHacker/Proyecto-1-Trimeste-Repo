using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 
    private int collectableCount;

    public TextMeshProUGUI pointTMP;

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


    #endregion
}
