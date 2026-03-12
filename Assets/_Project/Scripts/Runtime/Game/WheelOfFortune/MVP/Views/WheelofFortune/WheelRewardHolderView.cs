using NaughtyAttributes;
using UnityEngine;

namespace Game.Views
{
    public class WheelRewardHolderView : MonoBehaviour
    {
        [field: SerializeField] public int SlotIndex { get; private set; }
        [field: SerializeField, ReadOnly] private WheelRewardSlotView RewardSlotView { get; set; }
        
        public void SetRewardSlotView(WheelRewardSlotView rewardSlotView)
        {
            RewardSlotView = rewardSlotView;
            RewardSlotView.SetParent(transform);
        }
    }
}