using System;
using UnityEngine;
using UnityEngine.UI;

public class BombNotificationComponent : BaseDialog
{
    [SerializeField] private Button giveUpButton;

    public event Action OnGiveUpButtonClicked;

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