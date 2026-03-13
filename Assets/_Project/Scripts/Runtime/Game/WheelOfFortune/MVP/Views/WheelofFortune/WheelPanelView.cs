using System;
using Cysharp.Threading.Tasks;
using Game.Configs;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Views
{
    public class WheelPanelView : MonoBehaviour, IWheelPanelView
    {
        public event Action OnInitialize;
        [field: SerializeField, ReadOnly] public bool IsInitialized { get; private set; }
        [field: SerializeField, ReadOnly] public WheelRewardHolderView[] RewardHolders { get; private set; }
        
        [field: SerializeField] private Image WheelSpinnerImage { get; set; }
        [field: SerializeField] private Image WheelIndicatorImage { get; set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            RewardHolders = GetComponentsInChildren<WheelRewardHolderView>();
            
            EditorUtility.SetDirty(this);
        }
#endif

        public void Initialize() => SetActive(true).ContinueWith(RaiseInitialized).Forget();

        public async UniTask SetActive(bool value)
        {
            // TODO: Use Prepared Animation Module
            gameObject.SetActive(value);
            await UniTask.Yield();
        }

        private void RaiseInitialized()
        {
            IsInitialized = true;
            OnInitialize?.Invoke();
        }

        public void UpdateWheelVisuals(WheelVisualData wheelVisualData)
        {
            WheelSpinnerImage.sprite = wheelVisualData.SpinnerSprite;
            WheelIndicatorImage.sprite = wheelVisualData.IndicatorSprite;
        }
    }
}