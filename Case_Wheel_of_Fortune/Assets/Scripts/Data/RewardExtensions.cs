using System.Linq;

namespace Data
{
    public static class RewardExtensions
    {
        public static RewardModel ToModel(this StageRewardModel model)
        {
            return new RewardModel()
            {
                Amount =  model.amount,
                Type = model.assetReference.type,
                Sprite =  model.assetReference.sprite,
                UniqueKey =   model.assetReference.uniqueKey,
            };
        } 
        
        public static StageModel ToModel(this StageModelSO model)
        {
            return new StageModel()
            {
                Type = model.type,
                Index =  model.index,
                Rewards = model.rewards.Select(ToModel).ToList(),
            };
        } 
    }
}