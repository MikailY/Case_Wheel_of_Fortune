using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinContainerComponentWidget : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;

    public void Set(RewardModel model)
    {
        image.sprite = model.Sprite;
        text.text = $"x{model.Amount}";
    }
}