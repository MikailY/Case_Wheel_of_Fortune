using UnityEngine;

namespace WheelOfFortune.Data
{
    [CreateAssetMenu(menuName = "Create RewardAssetSO", fileName = "RewardAssetSO", order = 0)]
    public class RewardAssetSO : ScriptableObject
    {
        public string uniqueKey;
        public Sprite sprite;
    }
}