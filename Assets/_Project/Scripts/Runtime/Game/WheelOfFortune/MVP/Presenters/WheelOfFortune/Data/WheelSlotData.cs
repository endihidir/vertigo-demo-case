using System;
using System.Collections.Generic;
using System.Globalization;
using Core.Extensions;
using Core.Utils;
using Game.Enums;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class WheelSlotData
    {
        [field: SerializeField] public int SlotIndex { get; private set; }
        [field: SerializeField] public WheelSlotType WheelSlotType { get; private set; }
        [field: SerializeField, HideIf(nameof(IsBomb)), AllowNesting] public RewardDefinition RewardDefinition { get; private set; }
        [field: SerializeField] public List<WeightThreshold> WeightThresholds { get; private set; }
        public bool IsBomb => WheelSlotType == WheelSlotType.Bomb;

        public float GetWeight(int zoneCount)
        {
            WeightThreshold active = null;

            foreach (var threshold in WeightThresholds)
            {
                if (threshold.ZoneCount <= zoneCount)
                    active = threshold;
                else
                    break;
            }
            
            var defWeight = IsBomb ? 0f : 1f;
            
            var weight = active?.Weight ?? defWeight;
            
            return weight > 0f ? weight : defWeight;
        }

        public string GetValueFormat(int value)
        {
            return RewardDefinition.ValueType switch
            {
                RewardValueType.Numeric   => value.HideBigNumber(CultureInfo.InvariantCulture),
                RewardValueType.Stackable => $"X{value.HideBigNumber(CultureInfo.InvariantCulture)}",
                _                         => string.Empty
            };
        }
    }
    
    [Serializable]
    public class WeightThreshold
    {
        [field: SerializeField] public int ZoneCount { get; private set; }
        [field: SerializeField] public float Weight { get; private set; }
    }
}