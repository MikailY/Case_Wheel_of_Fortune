using System.Collections.Generic;
using UnityEngine;

public class StagesContainerComponent : MonoBehaviour
{
    //TODO POOLING!!!!!!!
    
    [SerializeField] private Transform targetTransform;
    [SerializeField] private StagesContainerComponentWidget widgetPrefab;

    public void Set(List<StageModel> modelStages)
    {
        foreach (var modelStage in modelStages)
        {
            var widget = Instantiate(widgetPrefab, targetTransform);

            widget.Set(modelStage.Index, modelStage.Type);
        }
    }
}