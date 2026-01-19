using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardsContainerComponentWidget : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    public void Set(RewardModel model)
    {
        text.text = $"x{model.Amount}";
        image.sprite = model.Sprite;
    }
}