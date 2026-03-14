using Core.Extensions;
using Game.Configs;
using Game.Enums;
using UnityEngine;

namespace Game.Handlers
{
    public sealed class WheelRewardCalculator : IWheelRewardCalculator
    {
        private readonly WheelOfFortuneConfigContainerSO _configContainer;

        public WheelRewardCalculator(WheelOfFortuneConfigContainerSO configContainer)
        {
            _configContainer = configContainer;
        }

        public int Calculate(int slotIndex, WheelType wheelType, int zoneCounter)
        {
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

        public string FormatValue(int slotIndex, WheelType wheelType, int calculatedValue)
        {
            var definition = _configContainer.GetWheelConfig(wheelType).GetWheelSlotData(slotIndex).RewardDefinition;
            
            return definition.ValueType switch
            {
                RewardValueType.Numeric   => calculatedValue.HideBigNumber(),
                RewardValueType.Stackable => $"X{calculatedValue.HideBigNumber()}",
                _                         => string.Empty
            };
        }
    }
}