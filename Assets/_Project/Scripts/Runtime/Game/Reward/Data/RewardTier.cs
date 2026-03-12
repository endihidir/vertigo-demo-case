using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Data
{
    [Serializable]
    public class RewardTier
    {
        [field: FormerlySerializedAs("<TypeId>k__BackingField")] [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public List<RewardVisualDataSO> RewardVisuals { get; private set; }

        public RewardVisualDataSO GetVisual(int amount)
        {
            RewardVisualDataSO result = RewardVisuals[0];

            foreach (var threshold in RewardVisuals)
            {
                if (amount >= threshold.MinAmount)
                    result = threshold;
            }

            return result;
        }
    }
}