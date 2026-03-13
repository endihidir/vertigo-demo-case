using System;
using Core.Attributes;
using Game.Enums;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public struct RewardDefinition
    {
        [field: SerializeField, ConstantDropdown(typeof(RewardId))] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public RewardCategory Category { get; private set; }
        [field: SerializeField] public RewardValueType ValueType { get; private set; }
        [field: SerializeField, HideIf(nameof(IsUniqueItem)), AllowNesting] public int BaseValue { get; private set; }
        [field: SerializeField] public string Label { get; private set; }
        
        public bool IsUniqueItem => ValueType == RewardValueType.Unique;
        
        public RewardDefinition(string id, string name, RewardCategory category, RewardValueType valueType, int baseValue, string label)
        {
            Id = id;
            Name = name;
            Category = category;
            ValueType = valueType;
            BaseValue = baseValue;
            Label = label;
        }
    }
}
