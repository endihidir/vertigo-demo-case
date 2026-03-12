using System;
using Game.Data;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public class WheelSlotData
    {
        [field: SerializeField] public int SlotIndex { get; private set; }
        [field: SerializeField] public RewardDefinition RewardDefinition { get; private set; }
    }
}