using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "RewardVisualConfig", menuName = "Game/Configs/RewardVisualConfig")]
    public class RewardVisualConfigSO : ScriptableObject
    {
        [field: SerializeField] public List<RewardTier> RewardVisuals { get; private set; }

        public RewardVisualDataSO GetVisual(string id, int amount)
        {
            var config = RewardVisuals.FirstOrDefault(x => x.Id == id);
            return config?.GetVisual(amount);
        }
    }
}