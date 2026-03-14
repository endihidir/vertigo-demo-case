using System.Collections.Generic;
using Core.SaveSystem;

namespace Game.Data
{
    public sealed class WheelRewardDatabase : IWheelRewardDatabase
    {
        private const string SAVE_KEY = "wheel_reward_data";

        private readonly IJsonSaveService _saveService;
        private readonly WheelRewardData _data;
        
        public List<RewardEntry> RewardEntries => _data.Entries;

        public WheelRewardDatabase()
        {
            _saveService = new JsonSaveService();
            _data = _saveService.LoadFromPrefs<WheelRewardData>(SAVE_KEY);
        }

        public int GetAmount(string itemId)
        {
            var entry = GetOrCreateEntry(itemId);
            return entry.Amount;
        }

        public void AddAmount(string itemId, int amount)
        {
            var entry = GetOrCreateEntry(itemId);
            entry.Amount += amount;
        }

        public void SaveRewards() => _saveService.SaveToPrefs(SAVE_KEY, _data);

        private RewardEntry GetOrCreateEntry(string itemId)
        {
            var entry = _data.Entries.Find(e => e.Id == itemId);

            if (entry != null) return entry;

            entry = new RewardEntry { Id = itemId, Amount = 0 };
            
            _data.Entries.Add(entry);

            return entry;
        }
    }
}
