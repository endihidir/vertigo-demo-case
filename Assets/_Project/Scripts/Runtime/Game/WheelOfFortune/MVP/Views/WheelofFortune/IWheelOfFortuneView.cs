using System;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Modules;
using UnityEngine.UI;

namespace Game.Views
{
    public interface IWheelOfFortuneView
    {
        event Action OnInitialize;
        Button SpinButton { get; }
        WheelSpinAnimationModule SpinAnimationModule { get; }
        WheelRewardHolderView[] RewardHolders { get; }
        void Initialize();
        UniTask SetActive(bool value);
        void UpdateWheelVisuals(WheelVisualData wheelVisualData);
        void SetSpinButtonInteractable(bool active);
    }
}