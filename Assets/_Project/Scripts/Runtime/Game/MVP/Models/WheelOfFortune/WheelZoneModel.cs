using System;
using Game.Configs;
using Game.Enums;

namespace Game.Models
{
    public sealed class WheelZoneModel : IWheelZoneModel
    {
        public event Action OnZoneUpdate;
        public int ZoneCounter { get; private set; } = 1;

        public WheelZoneModel(WheelOfFortuneConfigContainerSO wheelConfigContainer)
        {
            ZoneCounter = wheelConfigContainer.OverrideZoneCount ? wheelConfigContainer.CustomZoneCount : ZoneCounter;
        }

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
