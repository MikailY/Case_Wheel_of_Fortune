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
    public Sprite Sprite;
}

public enum DialogStates
{
    WaitingToSpin,
    Spinning,
}

public class WheelOfFortuneDialog : MonoBehaviour
{
    [SerializeField] private SpinContainerComponent spinContainerComponent;
    [SerializeField] private StagesContainerComponent stagesContainerComponent;
    [SerializeField] private RewardsContainerComponent rewardsContainerComponent;
    [SerializeField] private RewardAnimationComponent rewardAnimationComponent;
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
        var firstStage = model.Stages.ElementAtOrDefault(0);

        if (firstStage == null)
        {
            Debug.LogError("First stage is null");
            return;
        }

        _model = model;
        _currentStage = firstStage;

        _rewards.Clear();
        rewardsContainerComponent.Clear();
        stagesContainerComponent.Set(model.Stages);
        spinContainerComponent.Set(_currentStage);
        superZoneCounterComponent.Set(GetNextZoneIndexByType(2));
        safeZoneCounterComponent.Set(GetNextZoneIndexByType(1));

        _state = DialogStates.WaitingToSpin;
        spinContainerComponent.ShowSpinButton();
        rewardsContainerComponent.ShowExitButton();
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
        if (_state != DialogStates.WaitingToSpin) return;

        var index = Random.Range(0, 8);

        spinContainerComponent.Spin(index);

        _currentRollIndex = index;
        _state = DialogStates.Spinning;
        spinContainerComponent.HideSpinButton();
        rewardsContainerComponent.HideExitButton();
    }

    private void SpinContainerComponentOnOnSpinCompleted()
    {
        if (_currentStage.BombIndex == _currentRollIndex)
        {
            DOVirtual.DelayedCall(0.5f, () => { bombNotificationComponent.Show(); });
            return;
        }

        var rewardToCollect = _currentStage.Rewards.ElementAtOrDefault(_currentRollIndex);

        if (rewardToCollect == null)
        {
            Debug.LogError($"Current stage({_currentStage.Index}) reward({_currentRollIndex}) is null?");
            return;
        }

        RewardModel rewardAfterUpdate;

        rewardAnimationComponent.Set(rewardToCollect);

        if (_rewards.Exists(x => x.UniqueKey == rewardToCollect.UniqueKey))
        {
            var existingReward = _rewards.First(x => x.UniqueKey == rewardToCollect.UniqueKey);

            existingReward.Amount += rewardToCollect.Amount;

            rewardAfterUpdate = existingReward;
        }
        else
        {
            _rewards.Add(rewardToCollect);

            rewardsContainerComponent.InsertReward(rewardToCollect);

            rewardAfterUpdate = rewardToCollect;
        }

        var targetRectTransform = rewardsContainerComponent.GetTargetRectTransform(rewardToCollect.UniqueKey);

        rewardsContainerComponent.FocusTargetRectTransform(targetRectTransform);

        rewardAnimationComponent.MoveTo(targetRectTransform, () => OnRewardAnimationComplete(rewardAfterUpdate));

        return;

        void OnRewardAnimationComplete(RewardModel reward)
        {
            rewardsContainerComponent.UpdateReward(reward.UniqueKey, reward.Amount);

            var nextStage = _model.Stages.ElementAtOrDefault(_currentStage.Index);

            if (nextStage == null)
            {
                DOVirtual.DelayedCall(0.5f, () => { missionCompletedComponent.Show(); });
                return;
            }

            stagesContainerComponent.GoNext(nextStage.Index, () => OnNextStageAnimationFinished(nextStage));
        }

        void OnNextStageAnimationFinished(StageModel nextStage)
        {
            spinContainerComponent.Set(nextStage);
            superZoneCounterComponent.Set(GetNextZoneIndexByType(2));
            safeZoneCounterComponent.Set(GetNextZoneIndexByType(1));

            _currentStage = nextStage;
            _state = DialogStates.WaitingToSpin;
            spinContainerComponent.ShowSpinButton();
            rewardsContainerComponent.ShowExitButton();
        }
    }

    private void RewardsContainerComponentOnOnExitButtonClicked()
    {
        if (_state == DialogStates.Spinning) return;

        rewardsContainerComponent.HideExitButton();

        DOVirtual.DelayedCall(0.5f, () => Init(_model));
    }

    private void BombNotificationComponentOnOnGiveUpButtonClicked()
    {
        bombNotificationComponent.Hide();

        DOVirtual.DelayedCall(0.5f, () => Init(_model));
    }

    private void MissionCompletedComponentOnOnCollectRewardsButtonClicked()
    {
        missionCompletedComponent.Hide();

        DOVirtual.DelayedCall(0.5f, () => Init(_model));
    }

    private int GetNextZoneIndexByType(int type)
    {
        var nextZone = _model.Stages.FirstOrDefault(x => x.Type == type && x.Index > _currentStage.Index);
        return nextZone?.Index ?? -1;
    }
}