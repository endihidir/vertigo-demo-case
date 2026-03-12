using UnityEngine;

namespace Game.Views
{
    public class WheelRewardSlotView : BaseSlotView
    {
        [field: SerializeField] public int SlotIndex { get; private set; }

        public void SetParent(Transform parent) => transform.SetParent(parent, false);
    }
}