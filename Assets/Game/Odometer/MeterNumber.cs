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
    }
}