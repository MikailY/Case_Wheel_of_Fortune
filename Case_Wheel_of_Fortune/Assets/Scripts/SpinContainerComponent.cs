using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinContainerComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private Image spinBaseImage;
    [SerializeField] private Image spinIndicatorImage;
    [SerializeField] private Button spinButton;
    [SerializeField] private Transform rotateTarget;
    [SerializeField] private List<SpinContainerComponentWidget> widgets;

    public event Action OnSpinButtonClicked;
    public event Action OnSpinCompleted;

    public void Set(StageModel model)
    {
        rotateTarget.eulerAngles = Vector3.zero;

        switch (model.Type)
        {
            case 0:
                headerText.text = "BRONZE SPIN";
                headerText.color = Color.red;
                break;
            case 1:
                headerText.text = "SILVER SPIN";
                headerText.color = Color.gray;
                break;
            case 2:
                headerText.text = "GOLDEN SPIN";
                headerText.color = Color.yellow;
                break;
        }

        for (var i = 0; i < model.Rewards.Count; i++)
        {
            if (model.BombIndex == i)
            {
                widgets[i].Set();
            }
            else
            {
                widgets[i].Set(model.Rewards[i]);
            }
        }
    }

    private void OnValidate()
    {
        if (spinButton == null)
            spinButton = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        spinButton.onClick.AddListener(Click);
    }

    private void OnDisable()
    {
        spinButton.onClick.RemoveListener(Click);
    }

    private void Click()
    {
        OnSpinButtonClicked?.Invoke();
    }

    public void Spin(int index)
    {
        var rotValue = Vector3.forward * (360f / 8f);
        var angle = rotValue * index;
        rotateTarget.DORotate(Vector3.back * (360 * 5) + angle, 4f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutFlash)
            .OnComplete(() => OnSpinCompleted?.Invoke());
    }
}