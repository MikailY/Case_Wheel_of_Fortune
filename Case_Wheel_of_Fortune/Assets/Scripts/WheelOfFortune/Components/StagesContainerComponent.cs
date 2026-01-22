using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace WheelOfFortune.Components
{
    public class StagesContainerComponent : MonoBehaviour
    {
        [SerializeField] private RectTransform targetTransform;
        [SerializeField] private StagesContainerComponentWidget widgetPrefab;

        private readonly List<StagesContainerComponentWidget> _widgets = new();
        private StagesContainerComponentWidget _activeWidget;

        public void Set(List<StageModel> modelStages)
        {
            if (_widgets.Count > 0)
            {
                foreach (var widget in _widgets)
                    Destroy(widget.gameObject);

                _widgets.Clear();
            }

            foreach (var modelStage in modelStages)
            {
                var widget = Instantiate(widgetPrefab, targetTransform);

                widget.Set(modelStage.Index, modelStage.Type);
                
                _widgets.Add(widget);
            }

            SetActiveWidget(1);

            targetTransform.anchoredPosition = Vector2.zero;
        }

        public void MoveToNextStage(int index, Action onComplete)
        {
            _activeWidget?.Disable();

            var nextWidgetLocalPositionX =
                _widgets.First(x => x.Index == index).transform.localPosition.x;

            targetTransform.DOAnchorPosX(-nextWidgetLocalPositionX, 0.5f)
                .OnComplete(() =>
                {
                    SetActiveWidget(index);
                    onComplete?.Invoke();
                });
        }

        private void SetActiveWidget(int index)
        {
            _activeWidget = _widgets.FirstOrDefault(x => x.Index == index);
            _activeWidget?.Activate();
        }
    }
}