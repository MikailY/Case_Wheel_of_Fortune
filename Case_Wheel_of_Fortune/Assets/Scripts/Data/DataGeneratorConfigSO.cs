using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [CustomEditor(typeof(DataGeneratorConfigSO))]
    public class DataGeneratorConfigSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var config = (DataGeneratorConfigSO)target;

            if (GUILayout.Button($"Check For New Rewards"))
            {
                config.CheckForNewRewards();
            }

            base.OnInspectorGUI();
        }
    }

    [Serializable]
    public class RewardPerceivedValue
    {
        public RewardAssetSO asset;
        public int perceivedValue;
    }

    [CreateAssetMenu(menuName = "Create DataGeneratorConfig", fileName = "DataGeneratorConfig", order = 0)]
    public class DataGeneratorConfigSO : ScriptableObject
    {
        public WheelOfFortuneConfigSO wheelOfFortuneConfigSo;
        public List<RewardPerceivedValue> rewardPerceivedValues;

        // private void OnValidate()
        // {
        //     CheckForNewRewards();
        // }

        public void CheckForNewRewards()
        {
            if (!TryLoadRewardsAsset(out var rewardAssets) || rewardAssets.Count < 8)
            {
                Debug.LogWarning($"{nameof(RewardAssetSO)} list count({rewardAssets.Count}) is less than 8");

                return;
            }

            var nonExistentRewards =
                rewardAssets.FindAll(x => !rewardPerceivedValues.Exists(y => y.asset.uniqueKey == x.uniqueKey));

            if (nonExistentRewards.Count <= 0) return;

            rewardPerceivedValues.AddRange(nonExistentRewards.Select(x => new RewardPerceivedValue()
            {
                asset = x,
                perceivedValue = 1,
            }).ToList());

            return;

            static bool TryLoadRewardsAsset(out List<RewardAssetSO> rewardAssets)
            {
                var guids = AssetDatabase.FindAssets($"t:{nameof(RewardAssetSO)}");
                rewardAssets = new List<RewardAssetSO>();

                Debug.Log($"TryLoadRewardsAsset:guids = length : {guids?.Length}");

                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var asset = AssetDatabase.LoadAssetAtPath<RewardAssetSO>(path);
                    if (asset == null) continue;
                    rewardAssets.Add(asset);
                }

                return rewardAssets.Count > 0;
            }
        }
    }
}