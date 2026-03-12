using System;
using Cysharp.Threading.Tasks;

namespace Game.Views
{
    public interface IWheelPanelView
    {
        event Action OnInitialize;
        WheelRewardHolderView[] RewardHolders { get; }
        void Initialize();
        UniTask SetActive(bool value);
    }
}