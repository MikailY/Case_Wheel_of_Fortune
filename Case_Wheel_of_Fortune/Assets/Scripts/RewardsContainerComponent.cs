using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardsContainerComponent : MonoBehaviour
{
    //TODO POOLING!!!!!!!

    [SerializeField] private Transform targetTransform;
    [SerializeField] private RewardsContainerComponentWidget widgetPrefab;
    [SerializeField] private Button exitButton;

    public event Action OnExitButtonClicked;

    private readonly List<RewardsContainerComponentWidget> _widgets = new();

    public void Append(RewardModel model)
    {
        //TODO check if reward already exist, if so increase amount DONT RECREATE WIRDGET
        var widget = Instantiate(widgetPrefab, targetTransform);

        _widgets.Add(widget);

        widget.Set(model);
    }

    public void Clear()
    {
        foreach (var widget in _widgets)
        {
            Destroy(widget.gameObject);
        }

        _widgets.Clear();
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