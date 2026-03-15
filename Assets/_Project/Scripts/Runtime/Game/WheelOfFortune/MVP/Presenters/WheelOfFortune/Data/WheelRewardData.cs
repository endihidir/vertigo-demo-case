using System;
using System.Collections.Generic;

namespace Game.Data
{
    [Serializable]
    public class WheelRewardData
    {
        public List<RewardEntry> entries = new();
    }
    
    [Serializable]
    public class RewardEntry
    {
        public string id;
        public int amount;
    }
}