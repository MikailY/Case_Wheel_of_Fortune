using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SpinTypeConfigData
{
    public int type;
    public string headerText;
    public Color color;
    public Sprite baseSprite;
    public Sprite indicatorSprite;
}

public class SpinContainerComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private Image spinBaseImage;
    [SerializeField] private Image spinIndicatorImage;
    [SerializeField] private Button spinButton;
    [SerializeField] private Transform rotateTarget;
    [SerializeField] private ScaleEffectComponent scaleEffectComponent;
    [SerializeField] private List<SpinContainerComponentWidget> widgets;
    [SerializeField] private List<SpinTypeConfigData> typeConfigs;

    public event Action OnSpinButtonClicked;
    public event Action OnSpinCompleted;

    public void Set(StageModel model)
    {
        rotateTarget.eulerAngles = Vector3.zero;

        var typeConfig = typeConfigs.FirstOrDefault(x => x.type == model.Type);

        if (typeConfig == null)
        {
            Debug.LogError("typeData not found");
            return;
        }

        headerText.text = typeConfig.headerText;
        headerText.color = typeConfig.color;
        spinBaseImage.sprite = typeConfig.baseSprite;
        spinIndicatorImage.sprite = typeConfig.indicatorSprite;
        scaleEffectComponent.Show();

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

    public void Spin(int index)
    {
        var rotValue = Vector3.forward * (360f / 8f);
        var angle = rotValue * index;
        rotateTarget.DORotate(Vector3.back * (360 * 5) + angle, 4f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutFlash)
            .OnComplete(() => OnSpinCompleted?.Invoke());
    }

    public void ShowSpinButton()
    {
        spinButton.interactable = true;
    }

    public void HideSpinButton()
    {
        spinButton.interactable = false;
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
}