using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "RewardVisualConfigContainer", menuName = "Game/Containers/RewardVisualConfigContainer")]
    public class RewardVisualConfigContainerSO : ScriptableObject
    {
        [field: SerializeField] public List<RewardTier> RewardVisuals { get; private set; }

        public RewardVisualDataSO GetVisualData(string rewardId)
        {
            var config = RewardVisuals.FirstOrDefault(x => x.Id == rewardId);
            return config?.GetVisual();
        }

        public bool TryGetRewardVisualData(string rewardId, out RewardVisualDataSO rewardVisualData)
        {
            rewardVisualData = RewardVisuals.FirstOrDefault(x => x.Id == rewardId)?.GetVisual();
            return rewardVisualData;
        }
    }
}