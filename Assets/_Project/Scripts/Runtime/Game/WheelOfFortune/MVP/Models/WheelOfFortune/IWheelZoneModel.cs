using System;

namespace Game.Models
{
    public interface IWheelZoneModel
    {
        event Action OnZoneUpdate;
        int ZoneCounter { get; }
        void MoveNextZone();
        void ResetZone();
    }
}