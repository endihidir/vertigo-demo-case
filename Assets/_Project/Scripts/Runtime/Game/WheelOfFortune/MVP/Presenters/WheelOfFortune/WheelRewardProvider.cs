using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Data;
using Game.Enums;
using Game.Utils;
using Game.Views;
using UnityEngine;

namespace Game.Handlers
{
    public sealed class WheelRewardProvider : IWheelRewardProvider
    {
        private readonly WheelOfFortuneConfigContainerSO _configContainer;
        private readonly RewardVisualConfigContainerSO _rewardVisualContainer;

        public WheelRewardProvider(WheelOfFortuneConfigContainerSO configContainer, RewardVisualConfigContainerSO rewardVisualContainer)
        {
            _configContainer = configContainer;
            _rewardVisualContainer = rewardVisualContainer;
        }

        public WheelSlotData GetSlotData(int slotIndex, int zoneCounter)
        {
            var wheelType = WheelOfFortuneUtils.GetWheelType(zoneCounter);
            
            return _configContainer.GetWheelConfig(wheelType).GetWheelSlotData(slotIndex);
        }

        public void PrepareSpinResultView(int slotIndex, int zoneCounter, WheelSpinResultView spinResultView)
        {
            var wheelSlotData = GetSlotData(slotIndex, zoneCounter);
            
            if (wheelSlotData.IsBomb)
            {
                spinResultView.InitBombPanel();
            }
            else
            {
                var visualData = _rewardVisualContainer.GetVisualData(wheelSlotData.RewardDefinition.Id);
                var calculatedValue = CalculateValue(slotIndex, zoneCounter);
                var rewardTxt = FormatValue(slotIndex, calculatedValue, zoneCounter);
                spinResultView.InitRewardPanel(visualData.Icon, rewardTxt);
            }

            spinResultView.SetActive(true).Forget();
        }

        public int CalculateValue(int slotIndex, int zoneCounter)
        {
            var wheelType = WheelOfFortuneUtils.GetWheelType(zoneCounter);

            var definition = _configContainer.GetWheelConfig(wheelType).GetWheelSlotData(slotIndex).RewardDefinition;
            
            if (definition.IsUniqueItem) return 0;

            var wheelMultiplier = _configContainer.GetWheelConfig(wheelType).ValueMultiplier;

            return definition.ValueType switch
            {
                RewardValueType.Numeric   => Mathf.RoundToInt(definition.BaseValue * wheelMultiplier * zoneCounter),
                RewardValueType.Stackable => Mathf.RoundToInt(definition.BaseValue * wheelMultiplier),
                _                         => 0
            };
        }

        public string FormatValue(int slotIndex, int calculatedValue, int zoneCounter)
        {
            var wheelType = WheelOfFortuneUtils.GetWheelType(zoneCounter);
            
            return _configContainer.GetWheelConfig(wheelType).GetWheelSlotData(slotIndex).GetValueFormat(calculatedValue);
        }
    }
}