using System;

namespace Game.Views
{
    public interface IWheelOfFortuneView
    {
        event Action OnInitialize;
        WheelSpinView WheelSpinView { get; }
        WheelSpinResultView WheelSpinResultView { get; }
        WheelRewardCollectView WheelRewardCollectView { get; }
        void Initialize();
    }
}