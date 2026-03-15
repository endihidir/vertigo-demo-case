using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Views
{
    public class WheelOfFortuneView : MonoBehaviour, IWheelOfFortuneView
    {
        public event Action OnInitialize;
        [field: SerializeField, ReadOnly] public WheelSpinView WheelSpinView { get; private set; }
        [field: SerializeField, ReadOnly] public WheelSpinResultView WheelSpinResultView { get; private set; }
        [field: SerializeField, ReadOnly] public WheelRewardCollectView WheelRewardCollectView { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
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