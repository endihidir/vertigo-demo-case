using System;
using System.Collections.Generic;
using Core.Attributes;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class RewardTier
    {
        [field: SerializeField, ConstantDropdown(typeof(ItemId))] public string Id { get; private set; }
        [field: SerializeField] public List<RewardVisualDataSO> RewardVisuals { get; private set; }

        public RewardVisualDataSO GetVisual() => RewardVisuals[0];
    }
}