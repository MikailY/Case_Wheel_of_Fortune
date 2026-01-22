using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class ScaleEffectComponent : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private float scaleAmount = 1.1f;

        private Transform _target;
        private Vector3 _defaultScale;
        private Tweener _tween;

        private void Awake()
        {
            _target = target ? target : transform;

            _defaultScale = _target.localScale;
        }

        public void Show()
        {
            if (_tween.IsActive())
                _tween.Kill();

            _tween = _target.DOScale(_defaultScale * scaleAmount, duration)
                .ChangeStartValue(_defaultScale)
                .OnComplete(() => _target.DOScale(_defaultScale, duration));
        }
    }
}