using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfFortune.Components
{
    public class SpinContainerComponentWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image image;
        [SerializeField] private Sprite bombSprite;

        public void Set()
        {
            image.sprite = bombSprite;
            
            text.text = "";
        }

        public void Set(RewardModel model)
        {
            image.sprite = model.Sprite;
            
            text.text = $"x{model.Amount}";
        }
    }
}