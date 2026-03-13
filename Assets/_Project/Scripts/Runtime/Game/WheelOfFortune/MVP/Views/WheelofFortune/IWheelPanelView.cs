using System;
using Cysharp.Threading.Tasks;
using Game.Configs;

namespace Game.Views
{
    public interface IWheelPanelView
    {
        event Action OnInitialize;
        WheelRewardHolderView[] RewardHolders { get; }
        void Initialize();
        UniTask SetActive(bool value);
        void UpdateWheelVisuals(WheelVisualData wheelVisualData);
    }
}