using Game.Configs;
using Game.Data;

namespace Game.Handlers
{
    public interface IWheelRewardProvider
    {
        WheelSlotData GetRewardSlotData(int slotIndex, int zoneCounter);
        SpinResultData GetSpinResultData(int slotIndex, int zoneCounter);
        int CalculateValue(int slotIndex, int zoneCounter);
        string FormatValue(int slotIndex, int calculatedValue, int zoneCounter);
    }
}