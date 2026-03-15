using System.Linq;
using Game.Configs;
using Game.Utils;
using UnityEngine;

namespace Game.Handlers
{
    public sealed class WheelSpinResolver : IWheelSpinResolver
    {
        private readonly WheelOfFortuneConfigContainerSO _configContainer;

        public WheelSpinResolver(WheelOfFortuneConfigContainerSO configContainer)
        {
            _configContainer = configContainer;
        }

        public int ResolveSlotIndex(int zoneCounter)
        {
            var wheelType = WheelOfFortuneUtils.GetWheelType(zoneCounter);
            
            var slots = _configContainer.GetWheelConfig(wheelType).WheelSlotData;

            var totalWeight = slots.Sum(slot => slot.GetWeight(zoneCounter));

            var roll = Random.Range(0f, totalWeight);
            
            var cumulative = 0f;

            foreach (var slot in slots)
            {
                cumulative += slot.GetWeight(zoneCounter);
                
                if (roll <= cumulative)
                    return slot.SlotIndex;
            }

            return slots[^1].SlotIndex;
        }
    }
}