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
    public int BombIndex;
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
    [SerializeField] private SpinContainerComponent spinContainerComponent;
    [SerializeField] private StagesContainerComponent stagesContainerComponent;
    [SerializeField] private RewardsContainerComponent rewardsContainerComponent;

    private DialogStates _state;
    private WheelOfFortuneDialogModel _model;
    private StageModel _currentStage;
    private int _currentRollIndex;

    public void Init(WheelOfFortuneDialogModel model)
    {
        _state = DialogStates.Init;

        var firstStage = model.Stages.ElementAtOrDefault(0);

        if (firstStage == null)
        {
            Debug.LogError("First stage is null");
            return;
        }

        _model = model;
        _currentStage = firstStage;

        rewardsContainerComponent.Clear();
        stagesContainerComponent.Set(model.Stages);
        spinContainerComponent.Set(_currentStage);

        _state = DialogStates.WaitingToSpin;
    }

    private void OnEnable()
    {
        spinContainerComponent.OnSpinButtonClicked += SpinContainerComponentOnOnSpinButtonClicked;
        spinContainerComponent.OnSpinCompleted += SpinContainerComponentOnOnSpinCompleted;
        rewardsContainerComponent.OnExitButtonClicked += RewardsContainerComponentOnOnExitButtonClicked;
    }

    private void OnDisable()
    {
        spinContainerComponent.OnSpinButtonClicked -= SpinContainerComponentOnOnSpinButtonClicked;
        spinContainerComponent.OnSpinCompleted -= SpinContainerComponentOnOnSpinCompleted;
        rewardsContainerComponent.OnExitButtonClicked -= RewardsContainerComponentOnOnExitButtonClicked;
    }

    private void SpinContainerComponentOnOnSpinButtonClicked()
    {
        Debug.Log("WheelOfFortuneDialog:SpinContainerOnOnSpinButtonClicked");

        if (_state != DialogStates.WaitingToSpin)
        {
            return;
        }

        var index = Random.Range(0, 8);

        Debug.Log(
            $"WheelOfFortuneDialog:Spinning({index}=S{_currentStage.Index} {_currentStage.Rewards[index].Amount} {_currentStage.Rewards[index].UniqueKey})...");

        spinContainerComponent.Spin(index);

        _currentRollIndex = index;
        _state = DialogStates.Spinning;
    }

    private void SpinContainerComponentOnOnSpinCompleted()
    {
        Debug.Log("WheelOfFortuneDialog:SpinContainerOnOnSpinCompleted");

        if (_currentStage.BombIndex == _currentRollIndex)
        {
            //TODO OPEN BOMB EXPLODED DIALOG
            Debug.LogError("THERE IS A BOMB!!!!!!!!!");
            return;
        }

        rewardsContainerComponent.Append(_currentStage.Rewards.ElementAtOrDefault(_currentRollIndex));

        var nextStage = _model.Stages.ElementAtOrDefault(_currentStage.Index);

        if (nextStage == null)
        {
            Debug.LogError("Next stage is null (WIN OR BUG/ERROR)");
            return;
        }

        _currentStage = nextStage;
        spinContainerComponent.Set(_currentStage);
        _state = DialogStates.WaitingToSpin;
    }

    private void RewardsContainerComponentOnOnExitButtonClicked()
    {
        Debug.Log("WheelOfFortuneDialog:RewardsContainerComponentOnOnExitButtonClicked");
    }
}