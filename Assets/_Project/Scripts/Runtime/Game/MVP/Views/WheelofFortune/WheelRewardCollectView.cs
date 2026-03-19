using System;
using Core.Modules;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Views
{
    public class WheelRewardCollectView : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public Button CollectButton { get; private set; }
        [field: SerializeField, ReadOnly] public Button ContinueButton { get; private set; }
        [field: SerializeField, ReadOnly] public SizeAnimationModule SizeAnimationModule { get; private set; }
        [field: SerializeField] private Transform RewardContentTransform { get; set; }
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            SizeAnimationModule = GetComponentInChildren<SizeAnimationModule>();
            
            var allButtons = GetComponentsInChildren<Button>(true);
            
            foreach (var button in allButtons)
            {
                if(button.name.Contains("collect", StringComparison.OrdinalIgnoreCase)) CollectButton = button;
                if(button.name.Contains("continue", StringComparison.OrdinalIgnoreCase)) ContinueButton = button;
            }
            
            EditorUtility.SetDirty(this);
        }
#endif

        public async UniTask SetActiveAsync(bool value)
        {
            gameObject.SetActive(value);
            await SizeAnimationModule.SetScale(value ? Vector3.one : Vector3.zero, value ? .25f : 0f, ease: Ease.OutBack);
        }
        
        public void PlaceSlot(WheelCollectedRewardSlotView slotView) => slotView.SetParent(RewardContentTransform);
    }
}