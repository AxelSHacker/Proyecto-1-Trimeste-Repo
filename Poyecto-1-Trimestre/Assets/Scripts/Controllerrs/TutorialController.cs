using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;
   
    [SerializeField]
    
    

    void Start()
    {


    }

    void Update()
    {
        if (GameManager.Instance.collectableCount >= 15)
        {
            canvasGroup.SetEnable(true);
            
        }
            

    }

    public void ContinueTutorial()
    {
        SceneManager.LoadScene("Tutorial Platform");
    }



    private void OnDisable()
    {
        DataManager.Instance.CleaData();
    }


}
