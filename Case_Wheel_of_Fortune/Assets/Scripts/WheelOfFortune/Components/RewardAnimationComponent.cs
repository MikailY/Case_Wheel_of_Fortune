using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfFortune.Components
{
    public class RewardAnimationComponent : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Transform rectTransform;

        public void Set(RewardModel reward)
        {
            gameObject.SetActive(true);
            
            rectTransform.localPosition = Vector3.zero;

            text.text = $"x{reward.Amount}";
            
            image.sprite = reward.Sprite;
        }

        public void MoveTo(RectTransform targetTransform, Action onComplete)
        {
            var startPosition = rectTransform.position;

            DOVirtual.Float(0, 1, 0.5f,
                    value => { rectTransform.position = Vector3.Lerp(startPosition, targetTransform.position, value); })
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                    gameObject.SetActive(false);
                });
        }
    }
}