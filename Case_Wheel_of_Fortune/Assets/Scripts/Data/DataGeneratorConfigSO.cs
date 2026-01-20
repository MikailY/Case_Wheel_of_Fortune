using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
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
    }
}