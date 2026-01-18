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
        public int type;
        public StageRewardModel[] rewards = new StageRewardModel[8];
//     [SerializeField] private StageRewardModel[] rewards = new StageRewardModel[REWARD_LENGTH];
//
//     public int Index => index;
//     public List<StageRewardModel> Rewards => rewards.ToList();
//
//     private const int REWARD_LENGTH = 8;
//     
//     private void OnValidate()
//     {
//         if (rewards.Length == REWARD_LENGTH) return;
//
//         if (rewards.Length <= REWARD_LENGTH) return;
//         
//         rewards = rewards.Take(REWARD_LENGTH).ToArray();
//             
// #if UNITY_EDITOR
//         EditorUtility.SetDirty(this);
//         AssetDatabase.SaveAssets();
// #endif
//
//         // for (var i = rewards.Length; i < REWARD_LENGTH; i++)
//         // {
//         //     rewards.
//         // }
//     }
    }
}