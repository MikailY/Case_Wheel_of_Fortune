using Data;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private WheelOfFortuneDialog dialog;
    [SerializeField] private WheelOfFortuneConfigSO config;

    private void Start()
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
        
        dialog.Init(model);
    }
}