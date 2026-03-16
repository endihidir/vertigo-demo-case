using System;
using UnityEngine.UI;

namespace Game.Views
{
    public interface IWheelOfFortuneView
    {
        event Action OnInitialize;
        Button PlayButton { get; }
        WheelSpinView WheelSpinView { get; }
        WheelSpinResultView WheelSpinResultView { get; }
        WheelRewardCollectView WheelRewardCollectView { get; }
        void Initialize();
    }
}