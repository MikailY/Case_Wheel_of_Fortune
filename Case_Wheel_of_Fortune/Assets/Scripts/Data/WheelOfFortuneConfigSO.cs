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

        public List<StageModel>  Get()
        {
            return stageModels.Select(x => x.ToModel()).ToList();
        }
    
        private void OnValidate()
        {
            //validate collection
        
            //handle spin types | golden crashes silver
        }
    }
}