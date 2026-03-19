using System;
using System.Collections.Generic;
using System.Linq;
using Core.SaveSystem;

namespace Game.Data
{
    public sealed class WheelRewardDatabase : IWheelRewardDatabase
    {
        private const string SAVE_KEY = "wheel_reward_data";

        private readonly IJsonSaveService _saveService;
        private readonly WheelRewardData _data;
        
        public IReadOnlyList<RewardEntry> RewardEntries => _data.entries;
        
        public event Action<string> OnRewardAmountChanged;
        public event Action OnRewardsReset;

        public WheelRewardDatabase(IJsonSaveService saveService)
        {
            _saveService = saveService;
            _data = _saveService.LoadFromPrefs(SAVE_KEY, new WheelRewardData());
        }

        public int GetAmount(string itemId)
        {
            var entry = GetOrCreateEntry(itemId);
            return entry.amount;
        }

        public void AddAmount(string itemId, int amount)
        {
            var entry = GetOrCreateEntry(itemId);
            entry.amount += amount;
            OnRewardAmountChanged?.Invoke(itemId);
        }
        public void Reset()
        {
            if (_data.entries.Count == 0) return;
            _data.entries.Clear();
            OnRewardsReset?.Invoke();
        }

        public void SaveRewards() => _saveService.SaveToPrefs(SAVE_KEY, _data);
        
        private RewardEntry GetOrCreateEntry(string itemId)
        {
            var entry = _data.entries.FirstOrDefault(e => e.id == itemId);

            if (entry != null) return entry;

            entry = new RewardEntry { id = itemId, amount = 0 };
            
            _data.entries.Add(entry);

            return entry;
        }
    }
}
