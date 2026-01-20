using Data;
using UnityEngine;
using UnityEngine.UI;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private WheelOfFortuneDialog dialog;
    [SerializeField] private WheelOfFortuneConfigSO config;
    [SerializeField] private Button playButton;

    private void OnValidate()
    {
        if (playButton == null)
            playButton = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        playButton.onClick.AddListener(Click);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(Click);
    }

    private void Click()
    {
        var stages = config.Get();

        if (stages == null)
        {
            Debug.LogError("wofConfig is null");
            return;
        }

        var model = new WheelOfFortuneDialogModel
        {
            Stages = stages,
        };

// #if UNITY_EDITOR
//         //DISABLE BOMBS TEMPORARY FOR TESTING STUFF
//         foreach (var modelStage in model.Stages)
//         {
//             modelStage.BombIndex = -1;
//         }
// #endif

        dialog.Init(model);
    }
}