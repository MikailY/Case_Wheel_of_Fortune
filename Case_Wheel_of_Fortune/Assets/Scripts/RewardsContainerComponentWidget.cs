using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardsContainerComponentWidget : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    public string UniqueKey { get; private set; }

    public void Set(RewardModel model)
    {
        UniqueKey = model.UniqueKey;
        text.text = $"x{model.Amount}";
        image.sprite = model.Sprite;
    }

    public void UpdateAmount(int amount)
    {
        text.text = $"x{amount}";
    }
}