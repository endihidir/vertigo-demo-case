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
        [field: SerializeField, ConstantDropdown(typeof(ItemId))] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public RewardValueType ValueType { get; private set; }
        [field: SerializeField, HideIf(nameof(IsUniqueItem)), AllowNesting] public int BaseValue { get; private set; }
        [field: SerializeField] public string Label { get; private set; }
        
        public bool IsUniqueItem => ValueType == RewardValueType.Unique;
        
        public RewardDefinition(string id, string name, RewardValueType valueType, int baseValue, string label)
        {
            Id = id;
            Name = name;
            ValueType = valueType;
            BaseValue = baseValue;
            Label = label;
        }
    }
}
