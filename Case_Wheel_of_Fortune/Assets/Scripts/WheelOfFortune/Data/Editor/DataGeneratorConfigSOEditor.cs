using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WheelOfFortune.Data.Editor
{
    [CustomEditor(typeof(DataGeneratorConfigSO))]
    public class DataGeneratorConfigSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var config = (DataGeneratorConfigSO)target;

            if (GUILayout.Button($"Check For New Rewards"))
            {
                CheckForNewRewards(config);
            }

            base.OnInspectorGUI();
        }

        private static void CheckForNewRewards(DataGeneratorConfigSO config)
        {
            if (!TryLoadRewardsAsset(out var rewardAssets) || rewardAssets.Count < 8)
            {
                Debug.LogWarning($"{nameof(RewardAssetSO)} list count({rewardAssets.Count}) is less than 8");

                return;
            }

            var nonExistentRewards =
                rewardAssets.FindAll(x => !config.rewardPerceivedValues.Exists(y => y.asset.uniqueKey == x.uniqueKey));

            if (nonExistentRewards.Count <= 0) return;

            config.rewardPerceivedValues.AddRange(nonExistentRewards.Select(x => new RewardPerceivedValue()
            {
                asset = x,
                perceivedValue = 1,
            }).ToList());
        }

        private static bool TryLoadRewardsAsset(out List<RewardAssetSO> rewardAssets)
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(RewardAssetSO)}");

            rewardAssets = new List<RewardAssetSO>();

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