using UnityEngine;
using WheelOfFortune;
using WheelOfFortune.Data;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private WheelOfFortuneDialog dialog;
    [SerializeField] private WheelOfFortuneConfigSO config;

    private void Start()
    {
        var model = new WheelOfFortuneDialogModel
        {
            Stages = config.Get(),
        };

        dialog.Init(model);
    }
}