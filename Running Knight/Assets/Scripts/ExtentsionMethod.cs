
using UnityEngine;

public static class ExtentsionMethod
{
     public static void SetEnable(this CanvasGroup canvasGroup, bool value)
    {
        canvasGroup.alpha = value ? 1f : 0f;
        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
    }
}
