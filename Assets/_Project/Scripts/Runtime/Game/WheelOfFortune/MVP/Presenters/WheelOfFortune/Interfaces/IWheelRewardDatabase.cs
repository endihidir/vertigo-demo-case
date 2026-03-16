using System;
using System.Collections.Generic;

namespace Game.Data
{
    public interface IWheelRewardDatabase
    {
        event Action<string> OnRewardAmountChanged;
        event Action OnRewardsReset;
        IReadOnlyList<RewardEntry> RewardEntries { get; }
        int GetAmount(string itemId);
        void AddAmount(string itemId, int amount);
        void Reset();
        void SaveRewards();
    }
}