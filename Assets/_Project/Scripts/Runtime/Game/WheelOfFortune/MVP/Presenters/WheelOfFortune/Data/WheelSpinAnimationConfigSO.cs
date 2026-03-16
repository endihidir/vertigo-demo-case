using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "WheelSpinAnimationConfig", menuName = "Game/Configs/WheelSpinAnimationConfig")]
    public class WheelSpinAnimationConfigSO : ScriptableObject
    {
        [field: SerializeField] public bool UseUnscaledTime { get; set; } = true;
        [field: SerializeField] public float SpinDuration { get; private set; } = 6f;
        [field: SerializeField] public float IdleSpeed { get; private set; } = 30f;
        [field: SerializeField] public int FullRotationCount { get; private set; } = 2;
        [field: SerializeField] public AnimationCurve SpinCurve { get; private set; }
    }
}