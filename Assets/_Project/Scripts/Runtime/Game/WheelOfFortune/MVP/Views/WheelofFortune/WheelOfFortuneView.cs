using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Views
{
    public class WheelOfFortuneView : MonoBehaviour, IWheelOfFortuneView
    {
        [field: SerializeField, ReadOnly] public Button PlayButton { get; private set; }
        [field: SerializeField, ReadOnly] public WheelSpinView WheelSpinView { get; private set; }
        [field: SerializeField, ReadOnly] public WheelSpinResultView WheelSpinResultView { get; private set; }
        [field: SerializeField, ReadOnly] public WheelRewardCollectView WheelRewardCollectView { get; private set; }
        public event Action OnInitialize;

#if UNITY_EDITOR
        private void OnValidate()
        {
            var allButtons = GetComponentsInChildren<Button>();
            
            foreach (var button in allButtons)
            {
                if(button.name.Contains("play", StringComparison.OrdinalIgnoreCase)) PlayButton = button;
            }
            
            WheelSpinView = GetComponentInChildren<WheelSpinView>(true);
            WheelSpinResultView = GetComponentInChildren<WheelSpinResultView>(true);
            WheelRewardCollectView = GetComponentInChildren<WheelRewardCollectView>(true);
            EditorUtility.SetDirty(this);
        }
#endif

        public void Initialize() => WheelSpinView.SetActiveAsync(true).ContinueWith(RaiseInitialized).Forget();
        private void RaiseInitialized() => OnInitialize?.Invoke();
    }
}