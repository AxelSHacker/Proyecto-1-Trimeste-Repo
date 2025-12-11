using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    CanvasGroup continousCanvasGroup;
    [SerializeField]
    CanvasGroup exitToMenuCanvasGroup;

    [SerializeField]
    PlayerControllerEndLess playerControllerEndLess;

    public int points;

    private static TutorialController _tutorialController;

    public static TutorialController  Instance => _tutorialController;

    void Awake()
    {
        if (_tutorialController == null)
        {
            _tutorialController = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {

        continousCanvasGroup.SetEnable(false);
        exitToMenuCanvasGroup.SetEnable(false);

        if (FindAnyObjectByType<PlayerControllerEndLess>() != null)
        {
            playerControllerEndLess = FindAnyObjectByType<PlayerControllerEndLess>();
        }
        else { playerControllerEndLess = null; }
    }

    void Update()
    {
        PointController();
        if (GameManager.Instance.collectableCount >= 10)
        {
            
            continousCanvasGroup.SetEnable(true);

            if (GameManager.Instance.collectableCount >= 30)
            {
                continousCanvasGroup.SetEnable(true);
                DataManager.Instance.CleaData();
            }


            if (playerControllerEndLess != null)
            {
                playerControllerEndLess.autoMovement = false;
            }

        }


    }

    public void ContinueTutorial()
    {
        SceneManager.LoadScene("Tutorial Platform");
    }

    private void PointController()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial EndLess")
        {
            DataManager.Instance.tutorialScore = GameManager.Instance.collectableCount;
            DataManager.Instance.Save();
        }
        
    }

    private void OnDisable()
    {
        if ( SceneManager.GetActiveScene().name == "Tutorial Platform")
        {
            PlayerPrefs.DeleteKey("tutorialMaxScore");

        }
    }
}
        






