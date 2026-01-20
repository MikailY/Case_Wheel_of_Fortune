using DG.Tweening;
using UnityEngine;

public abstract class BaseDialog : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public void Show()
    {
        canvasGroup.blocksRaycasts = true;

        canvasGroup.DOFade(1, 0.5f)
            .OnComplete(() => { canvasGroup.interactable = true; });
    }

    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        canvasGroup.DOFade(0, 0.5f);
    }
}