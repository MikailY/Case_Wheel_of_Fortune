using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class StagesTypeConfigData
{
    public int type;
    public Color color;
    public Color disabledColor;
    public Color activeColor;
    public Color activeBackgroundColor;
}

public class StagesContainerComponentWidget : MonoBehaviour
{
    [SerializeField] private TMP_Text indexText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private List<StagesTypeConfigData> typeConfigs;

    public int Index { get; private set; }

    private StagesTypeConfigData _typeConfig;
    private Color _defaultBackgroundColor;

    public void Set(int index, int type)
    {
        _typeConfig = typeConfigs.FirstOrDefault(x => x.type == type);
        _defaultBackgroundColor = backgroundImage.color;

        indexText.text = index.ToString();
        indexText.color = _typeConfig.color;

        Index = index;
    }

    public void Activate()
    {
        indexText.DOColor(_typeConfig.activeColor, 0.5f);
        backgroundImage.DOColor(_typeConfig.activeBackgroundColor, 0.5f);
    }

    public void Disable()
    {
        indexText.DOColor(_typeConfig.disabledColor, 0.2f);
        backgroundImage.DOColor(_defaultBackgroundColor, 0.2f);
    }
}