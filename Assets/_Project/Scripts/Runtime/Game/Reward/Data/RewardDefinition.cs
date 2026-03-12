using System;
using Core.Attributes;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public struct RewardDefinition
    {
        [field: SerializeField, ConstantDropdown(typeof(RewardId))] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int BaseValue { get; private set; }
        [field: SerializeField] public string Label { get; private set; }
        
        public RewardDefinition(string id, string name, int baseValue, string label)
        {
            Id = id;
            Name = name;
            BaseValue = baseValue;
            Label = label;
        }
    }
}
