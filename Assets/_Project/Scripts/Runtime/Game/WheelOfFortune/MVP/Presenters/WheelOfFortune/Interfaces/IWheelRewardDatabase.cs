using System.Collections.Generic;

namespace Game.Data
{
    public interface IWheelRewardDatabase
    {
        List<RewardEntry> RewardEntries { get; }
        int GetAmount(string itemId);
        void AddAmount(string itemId, int amount);
        void SaveRewards();
    }
}