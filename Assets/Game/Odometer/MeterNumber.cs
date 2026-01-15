using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Odometer
{
    public class MeterNumber:MonoBehaviour
    {
        public int CurrentDigit { get; private set; } = -1;
        private const float degreesPerDigit = 36f;
        
        // Helper to set the initial position without animation
        public void SetDigitInstant(int digit)
        {
            CurrentDigit = digit;
            transform.localRotation = Quaternion.Euler(digit * degreesPerDigit, 0, 0);
        }
        
        public Tween GetRotationTween(int digit, float duration)
        {
            CurrentDigit = digit;
            var targetAngle = digit * degreesPerDigit;
        
            // We use RotateMode.Fast to ensure it takes the shortest path
            return transform.DOLocalRotate(new Vector3(targetAngle, 0, 0), duration, RotateMode.Fast)
                .SetEase(Ease.InOutSine); // Smooth start and stop for all wheels
        }
        
        public Tween GetLimitedRotationTween(int targetDigit, float duration)
        {
            CurrentDigit = targetDigit;
            float targetAngle = targetDigit * degreesPerDigit;

            // RotateMode.Fast ensures it never spins more than 180 degrees.
            // If you want it to always spin "downwards", use RotateMode.LocalAxisAdd 
            // and calculate the offset, but 'Fast' is the most standard mechanical feel.
            return transform.DOLocalRotate(new Vector3(targetAngle, 0, 0), duration, RotateMode.Fast)
                .SetEase(Ease.InOutQuart);
        }
        
        // This is called every frame during a fast spin
        public void UpdateRotationContinuous(float value)
        {
            // If your numbers are 0, 1, 2... in order:
            float targetAngle = value * degreesPerDigit;
        
            // We use localRotation directly for maximum performance during fast updates
            transform.localRotation = Quaternion.Euler(targetAngle, 0, 0);
        }
    }
}