using TMPro;
using UnityEngine;

public class NextZoneCounterComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private ScaleEffectComponent scaleEffectComponent;

    private int _index;

    public void Set(int index)
    {
        if (_index == index) return;

        if (index == -1)
        {
            canvasGroup.alpha = 0;
            _index = index;
            return;
        }

        canvasGroup.alpha = 1;
        text.text = index.ToString();

        scaleEffectComponent?.Show();

        _index = index;
    }
}