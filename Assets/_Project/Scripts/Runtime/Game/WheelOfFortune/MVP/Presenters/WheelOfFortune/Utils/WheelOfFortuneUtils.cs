using Game.Enums;

namespace Game.Utils
{
    public static class WheelOfFortuneUtils
    {
        public static WheelType GetWheelType(int zoneCount)
        {
            if (zoneCount % 30 == 0) return WheelType.Gold;
            if (zoneCount % 5 == 0) return WheelType.Silver;
            return WheelType.Bronze;
        }
        
        public static string GetTitle(WheelType wheelType, int zoneCount) => wheelType switch
        {
            WheelType.Silver => $"SILVER SPIN",
            WheelType.Bronze => $"ZONE {zoneCount}",
            WheelType.Gold => "GOLDEN SPIN",
            _ => string.Empty
        };
    }
}
