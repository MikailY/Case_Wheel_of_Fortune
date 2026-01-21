using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardsContainerComponentWidget : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    [SerializeField] private ScaleEffectComponent scaleEffectComponent;

    public string UniqueKey { get; private set; }

    public void Init(RewardModel model)
    {
        UniqueKey = model.UniqueKey;
        text.text = "0";
        image.sprite = model.Sprite;
    }

    public void UpdateAmount(int amount)
    {
        text.text = $"x{amount}";

        scaleEffectComponent.Show();
    }
}