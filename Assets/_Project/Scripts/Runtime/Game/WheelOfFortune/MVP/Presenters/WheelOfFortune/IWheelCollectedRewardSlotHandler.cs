using Game.Views;

namespace Game.Handlers
{
    public interface IWheelCollectedRewardSlotHandler
    {
        void PopulateSlotViews(out WheelCollectedRewardSlotView[] wheelSlotViews);
    }
}