using System;
using System.Collections.Generic;

namespace Game.Data
{
    [Serializable]
    public class WheelRewardData
    {
        public List<RewardEntry> Entries { get; } = new();
    }
    
    [Serializable]
    public class RewardEntry
    {
        public string Id { get; set; }
        public int Amount { get; set; }
    }
}