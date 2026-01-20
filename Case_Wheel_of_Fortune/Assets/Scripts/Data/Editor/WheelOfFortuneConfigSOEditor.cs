using UnityEditor;
using UnityEngine;

namespace Data.Editor
{
    [CustomEditor(typeof(WheelOfFortuneConfigSO))]
    public class WheelOfFortuneConfigSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var config = (WheelOfFortuneConfigSO)target;

            if (GUILayout.Button($"Re Validate Stages"))
            {
                config.ValidateStages();
            }

            base.OnInspectorGUI();
        }
    }
}