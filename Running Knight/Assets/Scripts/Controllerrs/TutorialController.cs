using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TutorialController : MonoBehaviour
{

    [SerializeField]
    PlayerControllerEndLess playerControllerEndLess;
    [SerializeField] string nombreScena;
    [SerializeField] string estaScena;
    [SerializeField] bool _endLess = true;
    [SerializeField] TextMeshProUGUI pointsText;
    private static TutorialController _tutorialController;

    public static TutorialController Instance => _tutorialController;

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


        if (FindAnyObjectByType<PlayerControllerEndLess>() != null)
        {
            playerControllerEndLess = FindAnyObjectByType<PlayerControllerEndLess>();
        }
        else
        {
            playerControllerEndLess = null;
        }
    }

    void Update()
    {
        if (pointsText != null)
        {
            pointsText.text = DataManager.Instance.actualGameScore.ToString();
        }
        if (GameManager.Instance.collectableCount >= 10 && _endLess)
        {
            _endLess = false;
            GameManager.Instance.AsignarListenerButton(nombreScena, GameManager.Instance.continueButton);
            GameManager.Instance.AsignarListenerButton(estaScena, GameManager.Instance.restartButton);
            GameManager.Instance.ContinueGame();
            if (playerControllerEndLess != null)
            {
                playerControllerEndLess.autoMovement = false;
            }
        }
    }
}







