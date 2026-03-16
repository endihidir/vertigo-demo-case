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

        public WheelSlotData GetRewardSlotData(int slotIndex, int zoneCounter)
        {
            var wheelType = WheelOfFortuneUtils.GetWheelType(zoneCounter);
            
            return _configContainer.GetWheelConfig(wheelType).GetWheelSlotData(slotIndex);
        }
        
        public SpinResultData GetSpinResultData(int slotIndex, int zoneCounter)
        {
            var slotData = GetRewardSlotData(slotIndex, zoneCounter);
    
            if (slotData.IsBomb) return new SpinResultData(isBomb: true);
    
            var visualData = _rewardVisualContainer.GetVisualData(slotData.RewardDefinition.Id);
            var calculatedValue = CalculateValue(slotIndex, zoneCounter);
            var rewardText = FormatValue(slotIndex, calculatedValue, zoneCounter);
    
            return new SpinResultData(isBomb: false, visualData.Icon, rewardText);
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