using NaughtyAttributes;
using UnityEngine;

namespace Game.Views
{
    public class WheelRewardHolderView : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public int SlotIndex { get; private set; }
        
        public void SetSlotIndex(int index) => SlotIndex = index;
    }
}