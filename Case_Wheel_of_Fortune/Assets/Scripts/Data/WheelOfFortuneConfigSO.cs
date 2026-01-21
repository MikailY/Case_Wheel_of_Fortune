using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create WheelOfFortuneConfigSO", fileName = "WheelOfFortuneConfigSO", order = 0)]
    public class WheelOfFortuneConfigSO : ScriptableObject
    {
        public int silverSpinRate;
        public int goldenSpinRate;
        public StageModelSO[] stageModels;

        public List<StageModel> Get()
        {
            return stageModels.Select(x => x.ToModel()).ToList();
        }

        private void OnValidate()
        {
            ValidateStages();
        }

        public void ValidateStages()
        {
            foreach (var stageModel in stageModels)
            {
                if (stageModel.index % goldenSpinRate == 0)
                {
                    stageModel.type = 2;
                    stageModel.bombIndex = -1;
                }
                else if (stageModel.index % silverSpinRate == 0 || stageModel.index == 1)
                {
                    stageModel.type = 1;
                    stageModel.bombIndex = -1;
                }
                else
                {
                    stageModel.type = 0;
                    stageModel.bombIndex = Random.Range(0, 8);
                }
            }
        }
    }
}