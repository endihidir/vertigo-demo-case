using System;
using System.Collections.Generic;
using Core.Extensions;
using Game.Data;
using Game.Enums;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public class WheelSlotData
    {
        [field: SerializeField] public int SlotIndex { get; private set; }
        [field: SerializeField] public WheelSlotType WheelSlotType { get; private set; }
        [field: SerializeField, HideIf(nameof(IsBomb)), AllowNesting] public RewardDefinition RewardDefinition { get; private set; }
        [field: SerializeField] public List<WeightThreshold> WeightThresholds { get; private set; }
        public bool IsBomb => WheelSlotType == WheelSlotType.Bomb;

        public float GetWeight(int zoneCounter)
        {
            WeightThreshold active = null;

            foreach (var threshold in WeightThresholds)
            {
                if (threshold.ZoneCount <= zoneCounter)
                    active = threshold;
                else
                    break;
            }

            var weight = active?.Weight ?? 1f;
            
            return weight > 0f ? weight : 1f;
        }

        public string GetValueFormat(int value)
        {
            return RewardDefinition.ValueType switch
            {
                RewardValueType.Numeric   => value.HideBigNumber(),
                RewardValueType.Stackable => $"X{value.HideBigNumber()}",
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