using Game.Data;
using UnityEngine;

namespace Game.Views
{
    public class WheelSlotView : BaseSlotView
    {
        [field: SerializeField] public int SlotIndex { get; private set; }
        [field: SerializeField] public RewardDefinition RewardDefinition { get; private set; }

        public void SetParent(Transform parent) => transform.SetParent(parent, false);

        public void ApplyData(int slotIndex, RewardDefinition rewardDefinition)
        {
            SlotIndex = slotIndex;
            RewardDefinition = rewardDefinition;
        }
    }
}