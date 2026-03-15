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
    }
}
