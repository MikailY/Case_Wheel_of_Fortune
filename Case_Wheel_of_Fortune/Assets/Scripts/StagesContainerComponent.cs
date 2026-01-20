using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class StagesContainerComponent : MonoBehaviour
{
    [SerializeField] private RectTransform targetTransform;
    [SerializeField] private StagesContainerComponentWidget widgetPrefab;

    private readonly List<StagesContainerComponentWidget> _widgets = new();
    private StagesContainerComponentWidget _activeWidget;
    private float _distanceBetweenWidgets = -1;

    public void Set(List<StageModel> modelStages)
    {
        if (_widgets.Count > 0)
        {
            foreach (var widget in _widgets)
            {
                Destroy(widget.gameObject);
            }

            _widgets.Clear();
        }

        foreach (var modelStage in modelStages)
        {
            //TODO POOLING!!!!!!!
            var widget = Instantiate(widgetPrefab, targetTransform);

            widget.Set(modelStage.Index, modelStage.Type);
            _widgets.Add(widget);
        }

        SetActive(1);

        targetTransform.anchoredPosition = Vector2.zero;
    }

    public void GoNext(int index)
    {
        if (_distanceBetweenWidgets < 0)
        {
            //TODO find another solution
            var first = _widgets.ElementAtOrDefault(0)?.transform as RectTransform;
            var second = _widgets.ElementAtOrDefault(1)?.transform as RectTransform;
            var distance = first?.anchoredPosition.x - second?.anchoredPosition.x;
            if (distance is null)
            {
                Debug.LogWarning("first second widget do not exist");
                distance = 0;
            }

            _distanceBetweenWidgets = distance.Value;
        }

        _activeWidget?.Disable();

        targetTransform.DOAnchorPosX(targetTransform.anchoredPosition.x + _distanceBetweenWidgets, 0.5f)
            .OnComplete(() => { SetActive(index); });
    }

    private void SetActive(int index)
    {
        _activeWidget = _widgets.FirstOrDefault(x => x.Index == index);
        _activeWidget?.Activate();
    }
}