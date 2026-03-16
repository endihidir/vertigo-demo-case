using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UI;
#endif

namespace Game.Views
{
    public class WheelRewardCollectView : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public Button CollectButton { get; private set; }
        [field: SerializeField, ReadOnly] public Button ContinueButton { get; private set; }
        [field: SerializeField] private Transform RewardContentTransform { get; set; }
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
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
            await UniTask.Yield();
        }
        
        public void SettleReward(WheelCollectedRewardSlotView slotView) => slotView.SetParent(RewardContentTransform);
    }
}