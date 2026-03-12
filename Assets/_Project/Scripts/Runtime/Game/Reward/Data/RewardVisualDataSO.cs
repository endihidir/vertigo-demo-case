using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "RewardVisualData", menuName = "Game/Configs/RewardVisualData")]
    public class RewardVisualDataSO : ScriptableObject
    {
        [field: SerializeField] public int MinAmount { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}