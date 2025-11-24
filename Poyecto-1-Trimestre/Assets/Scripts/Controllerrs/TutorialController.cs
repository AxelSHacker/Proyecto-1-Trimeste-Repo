using System.Data.Common;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup.alpha = 0;
    }
    private void OnDisable()
    { 


        DataManager.Instance.CleaData();
    }
}
