using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;
   
    [SerializeField]
    PlayerControllerEndLess playerCEL;
    

    void Start()
    {


    }

    void Update()
    {
        if (GameManager.Instance.collectableCount >= 15)
        {
            canvasGroup.SetEnable(true);
            playerCEL.autoMovement = false;
            
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
