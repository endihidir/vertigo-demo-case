using System;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Modules;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Views
{
    public class WheelSpinView : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public Button SpinButton { get; private set; }
        [field: SerializeField, ReadOnly] public WheelSpinAnimationModule SpinAnimationModule { get; private set; }
        [field: SerializeField, ReadOnly] public WheelRewardHolderView[] RewardHolders { get; private set; }
        
        [field: SerializeField] private Image WheelSpinnerImage { get; set; }
        [field: SerializeField] private Image WheelIndicatorImage { get; set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            RewardHolders = GetComponentsInChildren<WheelRewardHolderView>();
            
            SpinAnimationModule = GetComponentInChildren<WheelSpinAnimationModule>();
            
            var allButtons = GetComponentsInChildren<Button>();
            
            foreach (var button in allButtons)
            {
                if(button.name.Contains("spin", StringComparison.OrdinalIgnoreCase)) SpinButton = button;
            }
            
            EditorUtility.SetDirty(this);
        }
#endif
        
        public async UniTask SetActiveAsync(bool value)
        {
            gameObject.SetActive(value);
            await UniTask.Yield();
        }

        public void UpdateWheelVisuals(WheelVisualData wheelVisualData)
        {
            WheelSpinnerImage.sprite = wheelVisualData.SpinnerSprite;
            WheelIndicatorImage.sprite = wheelVisualData.IndicatorSprite;
        }
        
        public void SetSpinButtonInteractable(bool active) => SpinButton.interactable = active;
    }
}