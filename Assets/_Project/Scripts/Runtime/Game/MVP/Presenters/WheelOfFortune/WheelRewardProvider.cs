using Game.Configs;
using Game.Data;
using Game.Enums;
using Game.Utils;
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

        public WheelConfigSO GetWheelConfig(int zoneCounter)
        {
            var wheelType = WheelOfFortuneUtils.GetWheelType(zoneCounter);
            return _configContainer.GetWheelConfig(wheelType);
        }

        public SpinResultData GetSpinResultData(int zoneCounter, int slotIndex)
        {
            var slotData = GetWheelConfig(zoneCounter).GetWheelSlotData(slotIndex);
    
            if (slotData.IsBomb) return new SpinResultData(isBomb: true);
    
            var visualData = _rewardVisualContainer.GetVisualData(slotData.RewardDefinition.Id);
            var calculatedValue = CalculateValue(zoneCounter, slotIndex);
            var rewardText = slotData.GetValueFormat(calculatedValue);
    
            return new SpinResultData(isBomb: false, visualData.Icon, rewardText);
        }

        public int CalculateValue(int zoneCounter, int slotIndex)
        {
            var definition = GetWheelConfig(zoneCounter).GetWheelSlotData(slotIndex).RewardDefinition;
            
            if (definition.IsUniqueItem) return 0;

            var wheelMultiplier = GetWheelConfig(zoneCounter).ValueMultiplier;

            return definition.ValueType switch
            {
                RewardValueType.Numeric   => Mathf.RoundToInt(definition.BaseValue * wheelMultiplier * zoneCounter),
                RewardValueType.Stackable => Mathf.RoundToInt(definition.BaseValue * wheelMultiplier),
                _                         => 0
            };
        }
    }
}