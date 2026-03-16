using UnityEngine;

namespace Game.Data
{
    public readonly struct SpinResultData
    {
        public bool IsBomb { get; }
        public Sprite RewardIcon { get; }
        public string RewardText { get; }

        public SpinResultData(bool isBomb, Sprite rewardIcon = null, string rewardText = null)
        {
            IsBomb = isBomb;
            RewardIcon = rewardIcon;
            RewardText = rewardText;
        }
    }
}