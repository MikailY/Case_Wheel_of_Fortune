using System.Collections.Generic;
using Data;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor
{
    public class DataGeneratorEditorWindow : EditorWindow
    {
        private static DataGeneratorEditorWindow _currentWindow;

        private WheelOfFortuneConfigSO _configAsset;
        private List<RewardAssetSO> _rewardAssets;
        private int _amount;

        private const float LABEL_WIDTH = 100;
        private const float BUTTON_WIDTH = 200;
        private const float SPACING = 10;

        [MenuItem("Custom Windows/Data Generator", isValidateFunction: false, priority: 0)]
        static void OpenWindow()
        {
            _currentWindow = (DataGeneratorEditorWindow)GetWindow(typeof(DataGeneratorEditorWindow));
            _currentWindow.titleContent = new GUIContent("Data Generator");
            _currentWindow.minSize = new Vector2(300, 300);
            _currentWindow.maxSize = new Vector2(1200, 600);
            _currentWindow.Show();
        }

        private void OnEnable()
        {
            if (TryLoadConfigAsset(out var config))
            {
                _configAsset = config;
            }
            else
            {
                Debug.LogWarning($"Could not load {nameof(WheelOfFortuneConfigSO)}");
            }

            if (TryLoadRewardsAsset(out var rewardAssets))
            {
                if (rewardAssets.Count < 8)
                {
                    Debug.LogWarning($"{nameof(RewardAssetSO)} list count({rewardAssets.Count}) is less than 8");
                }

                _rewardAssets = rewardAssets;
            }
            else
            {
                Debug.LogWarning($"Could not load {nameof(RewardAssetSO)} list");
            }

            return;

            static bool TryLoadConfigAsset(out WheelOfFortuneConfigSO configAsset)
            {
                var guids = AssetDatabase.FindAssets($"t:{nameof(WheelOfFortuneConfigSO)}");

                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var asset = AssetDatabase.LoadAssetAtPath<WheelOfFortuneConfigSO>(path);
                    if (asset == null) continue;
                    configAsset = asset;
                    return true;
                }

                configAsset = null;
                return false;
            }

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

        private void OnGUI()
        {
            // // Klasör yoksa oluştur
            // if (!AssetDatabase.IsValidFolder(outputFolder))
            // {
            //     Directory.CreateDirectory(outputFolder);
            //     AssetDatabase.Refresh();
            // }
            _configAsset =
                EditorGUILayout.ObjectField(_configAsset, typeof(WheelOfFortuneConfigSO), false) as
                    WheelOfFortuneConfigSO;

            if (_configAsset == null)
            {
                if (!GUILayout.Button("Create Config Asset", GUILayout.Width(BUTTON_WIDTH))) return;

                var asset = ScriptableObject.CreateInstance<WheelOfFortuneConfigSO>();

                CreateAsset(asset, $"Assets/Data/{nameof(WheelOfFortuneConfigSO)}.asset");

                _configAsset = asset;

                return;
            }

            GUILayout.Space(SPACING);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                // if (GUILayout.Button($"Generate Rewards from ", GUILayout.Width(BUTTON_WIDTH)))
                // {
                //     AssetDatabase.DeleteAsset("Assets/Data/Rewards");
                //     AssetDatabase.CreateFolder("Assets/Data", "Rewards");
                //
                //     var guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/Textures/Ui Icons" });
                //
                //     Debug.Log($"guids = length : {guids?.Length}");
                //
                //     foreach (var guid in guids)
                //     {
                //         var texturePath = AssetDatabase.GUIDToAssetPath(guid);
                //         var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
                //
                //         if (sprite == null)
                //             continue;
                //
                //         var asset = ScriptableObject.CreateInstance<RewardAssetSO>();
                //         asset.sprite = sprite;
                //         asset.uniqueKey = sprite.name.Split("_").Skip(2)
                //             .Aggregate((s, s1) => $"{s.ToLowerInvariant()}_{s1.ToLowerInvariant()}");
                //
                //         AssetDatabase.CreateAsset(asset, $"Assets/Data/Rewards/{asset.uniqueKey}.asset");
                //     }
                //
                //     AssetDatabase.SaveAssets();
                //     AssetDatabase.Refresh();
                // }

                var guiEnable = GUI.enabled;

                if (GUILayout.Button($"Clear Stages", GUILayout.Width(BUTTON_WIDTH)))
                {
                    AssetDatabase.DeleteAsset("Assets/Data/Stages");
                    AssetDatabase.CreateFolder("Assets/Data", "Stages");
                }

                GUI.enabled = _amount > 0;

                if (GUILayout.Button($"Generate {_amount} Stage", GUILayout.Width(BUTTON_WIDTH)))
                {
                    AssetDatabase.DeleteAsset("Assets/Data/Stages");
                    AssetDatabase.CreateFolder("Assets/Data", "Stages");

                    var stages = new List<StageModelSO>();
                    for (var i = 0; i < _amount; i++)
                    {
                        var asset = ScriptableObject.CreateInstance<StageModelSO>();

                        for (var j = 0; j < 8; j++)
                        {
                            asset.rewards[j] = new StageRewardModel()
                            {
                                amount = j + 1,
                                assetReference = _rewardAssets[j],
                            };
                        }
                        AssetDatabase.CreateAsset(asset, $"Assets/Data/Stages/{nameof(StageModelSO)}{i}.asset");
                        stages.Add(asset);
                    }
                    _configAsset.stageModels = stages.ToArray();

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                GUI.enabled = guiEnable;
            }

            GUILayout.Space(SPACING);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Generate amount:", GUILayout.Width(LABEL_WIDTH));
                _amount = EditorGUILayout.IntField(_amount, GUILayout.ExpandWidth(true));
            }
        }

        private static void CreateAsset(Object asset, string path)
        {
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}