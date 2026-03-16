using Game.Enums;

namespace Game.Utils
{
    public static class WheelOfFortuneUtils
    {
        public static WheelType GetWheelType(int zoneCount)
        {
            if (zoneCount % 30 == 0) return WheelType.Gold;
            if (zoneCount % 5 == 0) return WheelType.Bronze;
            return WheelType.Silver;
        }
        
        public static string GetTitle(WheelType wheelType, int zoneCount) => wheelType switch
        {
            WheelType.Bronze => "BRONZE SPIN",
            WheelType.Gold => "GOLDEN SPIN",
            WheelType.Silver => $"ZONE {zoneCount}",
            _ => string.Empty
        };
    }
}
