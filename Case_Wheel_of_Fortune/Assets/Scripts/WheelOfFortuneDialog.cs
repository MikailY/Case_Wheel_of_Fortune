using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WheelOfFortuneDialogModel
{
    public List<StageModel> Stages = new();
}

public class StageModel
{
    public int Index;
    public int Type;
    public List<RewardModel> Rewards = new();
}

public class RewardModel
{
    public string UniqueKey;
    public int Amount;
    public int Type;
    public Sprite Sprite;
}

public enum DialogStates
{
    Init,
    WaitingToSpin,
    Spinning,
    GiveReward,
    Failed,
}

public class WheelOfFortuneDialog : MonoBehaviour
{
    [SerializeField] private SpinContainerComponent spinContainer;

    private DialogStates _state;
    private StageModel _currentStage;

    private void OnEnable()
    {
        spinContainer.OnSpinButtonClicked += SpinContainerOnOnSpinButtonClicked;
        spinContainer.OnSpinCompleted += SpinContainerOnOnSpinCompleted;
    }

    private void OnDisable()
    {
        spinContainer.OnSpinButtonClicked -= SpinContainerOnOnSpinButtonClicked;
        spinContainer.OnSpinCompleted -= SpinContainerOnOnSpinCompleted;
    }

    private void SpinContainerOnOnSpinButtonClicked()
    {
        Debug.Log("WheelOfFortuneDialog:SpinContainerOnOnSpinButtonClicked");

        if (_state != DialogStates.WaitingToSpin)
        {
            return;
        }

        var index = Random.Range(0, 8);

        Debug.Log(
            $"WheelOfFortuneDialog:Spinning({index}={_currentStage.Rewards[index].Amount} {_currentStage.Rewards[index].UniqueKey})...");

        spinContainer.Spin(index);

        _state = DialogStates.Spinning;
    }

    private void SpinContainerOnOnSpinCompleted()
    {
        Debug.Log("WheelOfFortuneDialog:SpinContainerOnOnSpinCompleted");

        _state = DialogStates.WaitingToSpin;
        spinContainer.Set(_currentStage);
    }

    public void Init(WheelOfFortuneDialogModel model)
    {
        _currentStage = model.Stages.FirstOrDefault();

        spinContainer.Set(_currentStage);

        _state = DialogStates.WaitingToSpin;
    }
}