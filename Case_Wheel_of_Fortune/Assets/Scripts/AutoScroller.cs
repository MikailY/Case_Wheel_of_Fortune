using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroller : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;

    public void ScrollToBottom()
    {
        StartCoroutine(ScrollNextFrame(0f));
    }

    public void ScrollToTop()
    {
        StartCoroutine(ScrollNextFrame(1f));
    }

    private IEnumerator ScrollNextFrame(float value)
    {
        yield return null;
        scrollRect.verticalNormalizedPosition = value;
    }
}