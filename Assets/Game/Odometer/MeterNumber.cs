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
        
        public Tween GetRotationTween(int digit, float duration,bool alwaysMove=true)
        {
           int steps = (CurrentDigit - digit + 10) % 10;
            if (alwaysMove && steps == 0) steps=10;
            var change = -(steps * degreesPerDigit);
            CurrentDigit = digit;
            return transform.DOLocalRotate(new Vector3(change, 0, 0), duration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutQuart); 
        }
        
    }
}