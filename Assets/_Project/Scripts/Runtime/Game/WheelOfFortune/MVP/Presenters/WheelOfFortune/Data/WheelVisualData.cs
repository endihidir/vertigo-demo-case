using System;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public struct WheelVisualData
    {
        [field: SerializeField] public Color TitleColor { get; private set; }
        [field: SerializeField] public Sprite IndicatorSprite { get; private set; }
        [field: SerializeField] public Sprite SpinnerSprite { get; private set; }
    }
}