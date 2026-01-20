using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class StageRewardModel
    {
        public int amount;
        public RewardAssetSO assetReference;
    }

    [CreateAssetMenu(menuName = "Create StageModelSO", fileName = "StageModelSO", order = 0)]
    public class StageModelSO : ScriptableObject
    {
        public int index;
        public int bombIndex;
        public int type;
        public StageRewardModel[] rewards = new StageRewardModel[8];
    }
}