using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WheelOfFortuneDialogModel
{
   public List<StageModel>  Stages = new();
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
public class WheelOfFortuneDialog : MonoBehaviour
{
    [SerializeField] private SpinContainer spinContainer;

    public void Init(WheelOfFortuneDialogModel model)   
    {
        Debug.Log(model.Stages.FirstOrDefault().Rewards.FirstOrDefault().UniqueKey);
    }
}