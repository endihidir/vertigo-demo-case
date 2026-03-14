using System;
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
        public bool IsBomb => WheelSlotType == WheelSlotType.Bomb;
    }
}