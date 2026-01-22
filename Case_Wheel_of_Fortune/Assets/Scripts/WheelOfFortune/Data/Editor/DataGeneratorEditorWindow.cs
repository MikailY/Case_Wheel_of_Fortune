using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace WheelOfFortune.Data.Editor
{
    public class DataGeneratorEditorWindow : EditorWindow
    {
        private static DataGeneratorEditorWindow _currentWindow;

        private DataGeneratorConfigSO _generatorConfig;
        private int _stagesToGenerateAmount;

        private const float LABEL_WIDTH = 100;
        private const float BUTTON_WIDTH = 200;
        private const float SPACING = 10;
        private const string ASSETS_DATA_PATH = "Assets/Data";
        private const string STAGES_FOLDER_NAME = "Stages";

        [MenuItem("Custom Windows/Data Generator", isValidateFunction: false, priority: 0)]
        private static void OpenWindow()
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
                _generatorConfig = config;
            }
            else
            {
                Debug.LogWarning($"Could not load {nameof(DataGeneratorConfigSO)}");
            }

            return;

            static bool TryLoadConfigAsset(out DataGeneratorConfigSO configAsset)
            {
                var guids = AssetDatabase.FindAssets($"t:{nameof(DataGeneratorConfigSO)}");

                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var asset = AssetDatabase.LoadAssetAtPath<DataGeneratorConfigSO>(path);
                    if (asset == null) continue;
                    configAsset = asset;
                    return true;
                }

                configAsset = null;
                return false;
            }
        }

        private void OnGUI()
        {
            _generatorConfig =
                EditorGUILayout.ObjectField(_generatorConfig, typeof(DataGeneratorConfigSO), false) as
                    DataGeneratorConfigSO;

            if (_generatorConfig == null)
            {
                if (!GUILayout.Button("Create Config Asset", GUILayout.Width(BUTTON_WIDTH))) return;

                var asset = ScriptableObject.CreateInstance<DataGeneratorConfigSO>();

                CreateAsset(asset, $"{ASSETS_DATA_PATH}/{nameof(DataGeneratorConfigSO)}.asset");

                _generatorConfig = asset;

                return;
            }

            GUILayout.Space(SPACING);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                var guiEnable = GUI.enabled;

                if (GUILayout.Button($"Clear Stages", GUILayout.Width(BUTTON_WIDTH)))
                {
                    ClearFolderAtPath($"{ASSETS_DATA_PATH}", $"{STAGES_FOLDER_NAME}");
                }

                GUI.enabled = _stagesToGenerateAmount > 0;

                if (GUILayout.Button($"Generate {_stagesToGenerateAmount} Stage", GUILayout.Width(BUTTON_WIDTH)))
                {
                    ClearFolderAtPath($"{ASSETS_DATA_PATH}", $"{STAGES_FOLDER_NAME}");

                    _generatorConfig.wheelOfFortuneConfigSo.stageModels =
                        GenerateStages(_generatorConfig.rewardPerceivedValues, _stagesToGenerateAmount);

                    _generatorConfig.wheelOfFortuneConfigSo.ValidateStages();

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                GUI.enabled = guiEnable;
            }

            GUILayout.Space(SPACING);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Generate amount:", GUILayout.Width(LABEL_WIDTH));
                _stagesToGenerateAmount =
                    EditorGUILayout.IntField(_stagesToGenerateAmount, GUILayout.ExpandWidth(true));
            }
        }

        private static void CreateAsset(Object asset, string path)
        {
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void ClearFolderAtPath(string path, string folderName)
        {
            AssetDatabase.DeleteAsset($"{path}/{folderName}");
            AssetDatabase.CreateFolder($"{path}", $"{folderName}");
        }

        private static StageModelSO[] GenerateStages(List<RewardPerceivedValue> rewardPerceivedValues, int amount)
        {
            var stages = new List<StageModelSO>();

            for (var i = 0; i < amount; i++)
            {
                var asset = ScriptableObject.CreateInstance<StageModelSO>();

                asset.index = i + 1;

                for (var j = 0; j < 8; j++)
                {
                    var rewardPerceivedValue =
                        rewardPerceivedValues[Random.Range(0, rewardPerceivedValues.Count)];

                    asset.rewards[j] = new StageRewardModel()
                    {
                        amount =
                            Math.Max(1,
                                (int)(Random.Range(1f * i, 10f * i) / rewardPerceivedValue.perceivedValue)),
                        assetReference = rewardPerceivedValue.asset,
                    };

                    rewardPerceivedValues.Remove(rewardPerceivedValue);
                }

                AssetDatabase.CreateAsset(asset,
                    $"{ASSETS_DATA_PATH}/{STAGES_FOLDER_NAME}/{nameof(StageModelSO)}{i + 1}.asset");

                stages.Add(asset);
            }

            return stages.ToArray();
        }
    }
}