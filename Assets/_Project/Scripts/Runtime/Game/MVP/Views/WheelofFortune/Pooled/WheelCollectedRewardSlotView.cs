using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class WheelCollectedRewardSlotView : BaseSlotView
    {
        [field: SerializeField] private Image Icon { get; set; }
        [field: SerializeField] private TextMeshProUGUI Value { get; set; }

        public void SetParent(Transform parent) => transform.SetParent(parent, false);
        public void SetImage(Sprite sprite) => Icon.sprite = sprite;
        public void SetValue(string text) => Value?.SetText(text);
        public void SetActiveValueTxt(bool value) => Value.gameObject.SetActive(value);
    }
}