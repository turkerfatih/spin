using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Odometer
{
    public class MeterNumber:MonoBehaviour
    {
        public int CurrentDigit { get; private set; } = -1;
        private const float degreesPerDigit = 36f;
        private float currentRotationX;
        
        // Helper to set the initial position without animation
        public void SetDigitInstant(int digit)
        {
            CurrentDigit = digit;
            transform.localRotation = Quaternion.Euler(digit * degreesPerDigit, 0, 0);
        }
        
        public Tween GetRotationTween(int digit, float duration,bool alwaysMove=true)
        {
           // int steps = (10-(digit - CurrentDigit + 10) % 10);
           int steps = (CurrentDigit - digit + 10) % 10;
            if (alwaysMove && steps == 0) steps=10;
            var change = -(steps * degreesPerDigit);
            currentRotationX += change;
     
            CurrentDigit = digit;
            return transform.DOLocalRotate(new Vector3(change, 0, 0), duration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutQuart); // Smooth start and stop for all wheels
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