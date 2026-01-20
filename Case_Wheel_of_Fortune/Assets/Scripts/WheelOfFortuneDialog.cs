using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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

public class WheelOfFortuneDialog : BaseDialog
{
    [SerializeField] private SpinContainerComponent spinContainerComponent;
    [SerializeField] private StagesContainerComponent stagesContainerComponent;
    [SerializeField] private RewardsContainerComponent rewardsContainerComponent;
    [SerializeField] private BombNotificationComponent bombNotificationComponent;
    [SerializeField] private MissionCompletedComponent missionCompletedComponent;
    [SerializeField] private NextZoneCounterComponent superZoneCounterComponent;
    [SerializeField] private NextZoneCounterComponent safeZoneCounterComponent;

    private DialogStates _state;
    private WheelOfFortuneDialogModel _model;
    private StageModel _currentStage;
    private int _currentRollIndex;
    private readonly List<RewardModel> _rewards = new();

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

        Show();

        rewardsContainerComponent.Clear();
        stagesContainerComponent.Set(model.Stages);
        spinContainerComponent.Set(_currentStage);
        superZoneCounterComponent.Set(GetNextZoneIndexByType(2));
        safeZoneCounterComponent.Set(GetNextZoneIndexByType(1));

        _state = DialogStates.WaitingToSpin;
    }

    private void OnEnable()
    {
        spinContainerComponent.OnSpinButtonClicked += SpinContainerComponentOnOnSpinButtonClicked;
        spinContainerComponent.OnSpinCompleted += SpinContainerComponentOnOnSpinCompleted;
        rewardsContainerComponent.OnExitButtonClicked += RewardsContainerComponentOnOnExitButtonClicked;
        bombNotificationComponent.OnGiveUpButtonClicked += BombNotificationComponentOnOnGiveUpButtonClicked;
        missionCompletedComponent.OnCollectRewardsButtonClicked +=
            MissionCompletedComponentOnOnCollectRewardsButtonClicked;
    }

    private void OnDisable()
    {
        spinContainerComponent.OnSpinButtonClicked -= SpinContainerComponentOnOnSpinButtonClicked;
        spinContainerComponent.OnSpinCompleted -= SpinContainerComponentOnOnSpinCompleted;
        rewardsContainerComponent.OnExitButtonClicked -= RewardsContainerComponentOnOnExitButtonClicked;
        bombNotificationComponent.OnGiveUpButtonClicked -= BombNotificationComponentOnOnGiveUpButtonClicked;
        missionCompletedComponent.OnCollectRewardsButtonClicked -=
            MissionCompletedComponentOnOnCollectRewardsButtonClicked;
    }

    private void SpinContainerComponentOnOnSpinButtonClicked()
    {
        Debug.Log("WheelOfFortuneDialog:SpinContainerOnOnSpinButtonClicked");

        if (_state != DialogStates.WaitingToSpin) return;

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
            bombNotificationComponent.Show();
            return;
        }

        var reward = _currentStage.Rewards.ElementAtOrDefault(_currentRollIndex);

        if (reward == null)
        {
            Debug.LogError("Reward is null???");
            return;
        }

        if (_rewards.Exists(x => x.UniqueKey == reward.UniqueKey))
        {
            var rewardIndex = _rewards.FindIndex(x => x.UniqueKey == reward.UniqueKey);
            var existingReward = _rewards[rewardIndex];
            existingReward.Amount += reward.Amount;
            _rewards[rewardIndex].Amount = existingReward.Amount;
            rewardsContainerComponent.UpdateReward(existingReward.UniqueKey, existingReward.Amount);
        }
        else
        {
            _rewards.Add(reward);
            rewardsContainerComponent.InsertReward(reward);
        }

        var nextStage = _model.Stages.ElementAtOrDefault(_currentStage.Index);

        if (nextStage == null)
        {
            missionCompletedComponent.Show();
            return;
        }

        stagesContainerComponent.GoNext(nextStage.Index);
        spinContainerComponent.Set(nextStage);
        superZoneCounterComponent.Set(GetNextZoneIndexByType(2));
        safeZoneCounterComponent.Set(GetNextZoneIndexByType(1));

        _currentStage = nextStage;
        _state = DialogStates.WaitingToSpin;
    }

    private void RewardsContainerComponentOnOnExitButtonClicked()
    {
        if (_state == DialogStates.Spinning) return;

        Debug.Log("WheelOfFortuneDialog:RewardsContainerComponentOnOnExitButtonClicked");

        Hide();
    }

    private void BombNotificationComponentOnOnGiveUpButtonClicked()
    {
        Hide();

        bombNotificationComponent.Hide();
    }

    private void MissionCompletedComponentOnOnCollectRewardsButtonClicked()
    {
        Hide();

        missionCompletedComponent.Hide();
    }

    private int GetNextZoneIndexByType(int type)
    {
        var nextZone = _model.Stages.FirstOrDefault(x => x.Type == type && x.Index > _currentStage.Index);
        return nextZone?.Index ?? -1;
    }
}