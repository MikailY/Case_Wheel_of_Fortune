using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BombNotificationComponent : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button giveUpButton;

    public event Action OnGiveUpButtonClicked;

    public void Show()
    {
        Debug.LogError("THERE IS A BOMB!!!!!!!!!");

        canvasGroup.DOFade(1, 1)
            .OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
            });
    }

    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        canvasGroup.DOFade(0, 1);
    }

    private void OnValidate()
    {
        if (giveUpButton == null)
            giveUpButton = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        giveUpButton.onClick.AddListener(Click);
    }

    private void OnDisable()
    {
        giveUpButton.onClick.RemoveListener(Click);
    }

    private void Click()
    {
        OnGiveUpButtonClicked?.Invoke();
    }
}