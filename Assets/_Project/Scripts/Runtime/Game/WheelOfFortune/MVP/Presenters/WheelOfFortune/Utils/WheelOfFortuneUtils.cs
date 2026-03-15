using Game.Enums;

namespace Game.Utils
{
    public static class WheelOfFortuneUtils
    {
        public static WheelType GetWheelType(int zoneCounter)
        {
            if (zoneCounter % 30 == 0) return WheelType.Gold;
            if (zoneCounter % 5 == 0) return WheelType.Bronze;
            return WheelType.Silver;
        }
    }
}
