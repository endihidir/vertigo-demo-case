using Game.Configs;
using Game.Enums;
using Game.Views;

namespace Game.Handlers
{
    public interface IWheelSlotViewHandler
    {
        WheelSlotView[] WheelSlotViews { get; }
        WheelVisualData GetWheelVisuals(WheelType wheelType);
        void PopulateSlotViews(WheelType wheelType, out WheelSlotView[] wheelSlotViews);
    }
}