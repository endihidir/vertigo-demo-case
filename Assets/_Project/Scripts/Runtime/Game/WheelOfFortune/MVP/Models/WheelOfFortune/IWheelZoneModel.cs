using System;
using Game.Enums;

namespace Game.Models
{
    public interface IWheelZoneModel
    {
        event Action OnZoneUpdate;
        int ZoneCounter { get; }
        void MoveNextZone();
        void ResetZone();
        WheelType GetWheelType();
    }
}