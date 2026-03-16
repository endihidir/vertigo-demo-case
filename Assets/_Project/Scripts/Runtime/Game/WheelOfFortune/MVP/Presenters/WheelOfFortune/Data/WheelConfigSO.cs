using System.Collections.Generic;
using System.Linq;
using Game.Enums;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "WheelConfig", menuName = "Game/Configs/WheelConfig")]
    public class WheelConfigSO : ScriptableObject
    {
        [field: SerializeField] public WheelType WheelType { get; private set; }
        [field: SerializeField] public float ValueMultiplier { get; private set; } = 1f;
        [field: SerializeField] public WheelVisualData WheelVisuals { get; private set; }
        
        [field: SerializeField, ShowIf(nameof(IsSilver))] public Sprite BombIcon { get; private set; }
        [field: SerializeField, ShowIf(nameof(IsSilver))] public string BombLabel { get; private set; }
        
        [field: SerializeField] public List<WheelSlotData> WheelSlotData { get; private set; }
        
        
        public bool IsSilver => WheelType == WheelType.Silver;
        
        public WheelSlotData GetWheelSlotData(int slotIndex)
        { 
            var slotData = WheelSlotData.FirstOrDefault(data => data.SlotIndex == slotIndex);
            
            return slotData;
        }

        public bool TryGetWheelSlotData(int slotIndex, out WheelSlotData slotData)
        {
            var wheelSlotData = WheelSlotData.FirstOrDefault(data => data.SlotIndex == slotIndex);
            
            slotData = wheelSlotData;
            
            return wheelSlotData != null;
        }
    }
}