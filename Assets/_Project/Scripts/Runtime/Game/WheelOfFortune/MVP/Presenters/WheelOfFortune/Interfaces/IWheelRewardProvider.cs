using Game.Configs;
using Game.Views;

namespace Game.Handlers
{
    public interface IWheelRewardProvider
    {
        WheelSlotData GetSlotData(int slotIndex, int zoneCounter);
        void PrepareSpinResultView(int slotIndex, int zoneCounter, WheelSpinResultView spinResultView);
        int CalculateValue(int slotIndex, int zoneCounter);
        string FormatValue(int slotIndex, int calculatedValue, int zoneCounter);
    }
}