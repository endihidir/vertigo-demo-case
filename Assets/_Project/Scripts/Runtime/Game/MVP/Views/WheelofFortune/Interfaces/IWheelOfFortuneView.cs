using UnityEngine.UI;

namespace Game.Views
{
    public interface IWheelOfFortuneView
    {
        Button PlayButton { get; }
        WheelSpinView WheelSpinView { get; }
        WheelSpinResultView WheelSpinResultView { get; }
        WheelRewardCollectView WheelRewardCollectView { get; }
    }
}