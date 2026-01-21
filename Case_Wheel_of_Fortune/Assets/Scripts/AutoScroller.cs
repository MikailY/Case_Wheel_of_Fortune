using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroller : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;

    public void ScrollToBottom()
    {
        scrollRect.DOVerticalNormalizedPos(0f, 0.1f).SetEase(Ease.OutCubic);
    }

    public void ScrollToTop()
    {
        scrollRect.DOVerticalNormalizedPos(1f, 0.1f).SetEase(Ease.OutCubic);
    }

    public void ScrollToItem(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        var contentHeight = scrollRect.content.rect.height;
        var viewportHeight = scrollRect.viewport.rect.height;

        if (contentHeight <= viewportHeight) return;

        var targetLocalY = target.localPosition.y;
        var scrollableHeight = contentHeight - viewportHeight;
        var centerOffset = viewportHeight / 2f;

        var endValue = Mathf.Clamp01(1f + (targetLocalY + centerOffset) / scrollableHeight);

        scrollRect.DOVerticalNormalizedPos(endValue, 0.1f).SetEase(Ease.OutCubic);
    }
}