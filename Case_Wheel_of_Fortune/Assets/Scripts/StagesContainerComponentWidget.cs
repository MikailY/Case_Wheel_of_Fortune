using TMPro;
using UnityEngine;

public class StagesContainerComponentWidget : MonoBehaviour
{
    [SerializeField] private TMP_Text indexText;

    public void Set(int index, int type)
    {
        indexText.text = index.ToString();
        indexText.color = type switch
        {
            1 => Color.blue,
            2 => Color.yellow,
            _ => Color.black
        };
    }
}