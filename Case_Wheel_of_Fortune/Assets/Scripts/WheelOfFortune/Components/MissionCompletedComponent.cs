using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace WheelOfFortune.Components
{
    public class MissionCompletedComponent : BaseDialog
    {
        [SerializeField] private Button collectRewardsButton;

        public event Action OnCollectRewardsButtonClicked;

        private void OnValidate()
        {
            if (collectRewardsButton == null)
                collectRewardsButton = GetComponentInChildren<Button>();
        }

        private void OnEnable()
        {
            collectRewardsButton.onClick.AddListener(Click);
        }

        private void OnDisable()
        {
            collectRewardsButton.onClick.RemoveListener(Click);
        }

        private void Click()
        {
            OnCollectRewardsButtonClicked?.Invoke();
        }
    }
}