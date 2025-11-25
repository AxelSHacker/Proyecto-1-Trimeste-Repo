using System.Data.Common;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    GameObject endlessRuenner;
    [SerializeField]
    GameObject platform2D;
    void Start()
    {
        canvasGroup.alpha = 0;
    }

    void Update()
    {
        if (GameManager.Instance.collectableCount >= 15)
        {
            canvasGroup.alpha = 1;

        }
    }

    public void ContinueTutorial()
    {
        endlessRuenner.SetActive(false);
        platform2D.SetActive(true);
    }
            
        

    private void OnDisable()
    {
        DataManager.Instance.CleaData();
    }


}
