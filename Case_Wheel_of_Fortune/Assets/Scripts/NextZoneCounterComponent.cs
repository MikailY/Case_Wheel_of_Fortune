using TMPro;
using UnityEngine;

public class NextZoneCounterComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private CanvasGroup canvasGroup;

    public void Set(int index)
    {
        if (index == -1)
        {
            canvasGroup.alpha = 0;
            return;
        }

        canvasGroup.alpha = 1;
        text.text = index.ToString();
    }
}