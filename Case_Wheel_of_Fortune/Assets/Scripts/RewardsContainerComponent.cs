using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RewardsContainerComponent : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private RewardsContainerComponentWidget widgetPrefab;
    [SerializeField] private Button exitButton;
    [SerializeField] private AutoScroller autoScroller;

    public event Action OnExitButtonClicked;

    private readonly List<RewardsContainerComponentWidget> _widgets = new();

    public void InsertReward(RewardModel model)
    {
        //TODO POOLING!!!!!!!
        var widget = Instantiate(widgetPrefab, targetTransform);

        _widgets.Add(widget);

        widget.Set(model);

        autoScroller?.ScrollToBottom();
    }

    public void UpdateReward(string uniqueKey, int amount)
    {
        _widgets.FirstOrDefault(x => x.UniqueKey == uniqueKey)?.UpdateAmount(amount);
    }

    public void Clear()
    {
        foreach (var widget in _widgets)
        {
            Destroy(widget.gameObject);
        }

        _widgets.Clear();
    }

    public void ShowExitButton()
    {
        exitButton.gameObject.SetActive(true);
    }

    public void HideExitButton()
    {
        exitButton.gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        if (exitButton == null)
            exitButton = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        exitButton.onClick.AddListener(Click);
    }

    private void OnDisable()
    {
        exitButton.onClick.RemoveListener(Click);
    }

    private void Click()
    {
        OnExitButtonClicked?.Invoke();
    }
}