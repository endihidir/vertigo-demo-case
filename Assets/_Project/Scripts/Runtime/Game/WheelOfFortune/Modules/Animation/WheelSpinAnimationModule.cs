using DG.Tweening;
using UnityEngine;

namespace Game.Modules
{
    public class WheelSpinAnimationModule : MonoBehaviour
    {
        [field: SerializeField] private bool UseUnscaledTime { get; set; } = true;
        [field: SerializeField] private float SpinDuration { get; set; } = 2f;
        [field: SerializeField] private float IdleSpeed { get; set; } = 30f;
        [field: SerializeField] private int FullRotationCount { get; set; } = 5;
        [field: SerializeField] private AnimationCurve SpinCurve { get; set; }
        
        private const float SlotAngle = 45f;

        private Tween _spinTween;

        public void PlayIdle()
        {
            _spinTween?.Kill();

            _spinTween = transform.DOLocalRotate(new Vector3(0f, 0f, -360f), 360f / IdleSpeed, RotateMode.LocalAxisAdd)
                                  .SetEase(Ease.Linear)
                                  .SetLoops(-1, LoopType.Incremental)
                                  .SetUpdate(UseUnscaledTime);
        }

        public Tween SpinTo(int slotIndex)
        {
            _spinTween?.Kill();

            var targetRotation = new Vector3(0f, 0f, CalculateEndAngle(slotIndex));

            _spinTween = transform.DOLocalRotate(targetRotation, SpinDuration, RotateMode.FastBeyond360)
                                  .SetEase(SpinCurve)
                                  .SetUpdate(UseUnscaledTime);

            return _spinTween;
        }
        
        public void KillAnimation() => _spinTween?.Kill();

        private float CalculateEndAngle(int slotIndex)
        {
            var targetAngle = -slotIndex * SlotAngle;
            var currentAngle = transform.localEulerAngles.z;
            if (currentAngle > 180f) currentAngle -= 360f;
            var delta = targetAngle - currentAngle;
            while (delta > 0f) delta -= 360f;
            delta -= FullRotationCount * 360f;
            return currentAngle + delta;
        }

        private void OnDestroy() => KillAnimation();
    }
}