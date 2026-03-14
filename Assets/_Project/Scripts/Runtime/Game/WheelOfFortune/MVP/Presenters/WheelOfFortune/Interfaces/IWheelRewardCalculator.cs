using Game.Enums;

namespace Game.Handlers
{
    public interface IWheelRewardCalculator
    {
        int Calculate(int slotIndex, WheelType wheelType, int zoneCounter);
        string FormatValue(int slotIndex, WheelType wheelType, int calculatedValue);
    }
}