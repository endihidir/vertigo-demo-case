using DG.Tweening;
using Game.Configs;
using UnityEngine;

namespace Game.Modules
{
    public class WheelSpinAnimationModule : MonoBehaviour
    {
        [field: SerializeField] private WheelSpinAnimationConfigSO Config { get; set; }
        
        private const float SlotAngle = 45f;

        private Tween _spinTween;

        public void PlayIdle()
        {
            _spinTween?.Kill();

            _spinTween = transform.DOLocalRotate(new Vector3(0f, 0f, -360f), 360f / Config.IdleSpeed, RotateMode.LocalAxisAdd)
                                  .SetEase(Ease.Linear)
                                  .SetLoops(-1, LoopType.Incremental)
                                  .SetUpdate(Config.UseUnscaledTime);
        }

        public Tween SpinTo(int slotIndex)
        {
            _spinTween?.Kill();

            var targetRotation = new Vector3(0f, 0f, CalculateEndAngle(slotIndex));

            _spinTween = transform.DOLocalRotate(targetRotation, Config.SpinDuration, RotateMode.FastBeyond360)
                                  .SetEase(Config.SpinCurve)
                                  .SetUpdate(Config.UseUnscaledTime);

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
            delta -= Config.FullRotationCount * 360f;
            return currentAngle + delta;
        }

        private void OnDestroy() => KillAnimation();
    }
}