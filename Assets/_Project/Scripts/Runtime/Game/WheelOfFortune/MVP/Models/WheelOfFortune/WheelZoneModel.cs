using System;
using Game.Enums;

namespace Game.Models
{
    public sealed class WheelZoneModel : IWheelZoneModel
    {
        public event Action OnZoneUpdate;
        public int ZoneCounter { get; private set; } = 1;

        public void MoveNextZone()
        {
            ZoneCounter++;
            
            OnZoneUpdate?.Invoke();
        }
        
        public void ResetZone()
        {
            ZoneCounter = 1;
            
            OnZoneUpdate?.Invoke();
        }

        public WheelType GetWheelType()
        {
            if (ZoneCounter % 30 == 0) return WheelType.Gold;
            if (ZoneCounter % 5 == 0) return WheelType.Bronze;
            return WheelType.Silver;
        }
    }
}
