using Game.Configs;
using Game.Data;

namespace Game.Handlers
{
    public interface IWheelRewardProvider
    {
        WheelConfigSO GetWheelConfig(int zoneCounter);
        SpinResultData GetSpinResultData(int zoneCounter, int slotIndex);
        int CalculateValue(int zoneCounter, int slotIndex);
    }
}