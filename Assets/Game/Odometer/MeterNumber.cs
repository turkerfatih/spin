using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Odometer
{
    public class MeterNumber:MonoBehaviour
    {
        public int CurrentDigit { get; private set; } = -1;
        private const float DegreesPerDigit = 36f;
        [SerializeField] private float Duration = 0.25f;
        
        // Helper to set the initial position without animation
        public void SetDigitInstant(int digit)
        {
            CurrentDigit = digit;
            transform.localRotation = Quaternion.Euler(digit * DegreesPerDigit, 0, 0);
        }
        
        public Tween GetRotationTween(int digit, float duration)
        {
            CurrentDigit = digit;
            float targetAngle = digit * DegreesPerDigit;
        
            // We use RotateMode.Fast to ensure it takes the shortest path
            return transform.DOLocalRotate(new Vector3(targetAngle, 0, 0), duration, RotateMode.Fast)
                .SetEase(Ease.InOutSine); // Smooth start and stop for all wheels
        }
    }
}