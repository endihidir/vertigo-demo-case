using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class WheelSlotView : BaseSlotView
    {
        [field: SerializeField, ReadOnly] public int SlotIndex { get; private set; }
        [field: SerializeField] private Image Icon { get; set; }
        [field: SerializeField] private TextMeshProUGUI Value { get; set; }

        public void SetParent(Transform parent) => transform.SetParent(parent, false);
        public void SetImage(Sprite sprite) => Icon.sprite = sprite;
        public void SetValue(string text) => Value?.SetText(text);
        public void SetSlotIndex(int slotIndex) => SlotIndex = slotIndex;
        public void SetActiveValueTxt(bool value) => Value.gameObject.SetActive(value);

        protected override void OnDeactivate()
        {
            Value?.SetText(string.Empty);
        }
    }
}