using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Game.Odometer
{
    public class MeterController:MonoBehaviour
    {
        public MeterNumber[] Segments; // 0: Units, 1: Tens, 2: Hundreds
        public int CurrentValue = 249;
    
        [Header("Settings")]
        public float AnimationDuration = 0.4f; // Total time for all wheels to finish
        public int DelayBetweenTicksMs = 100;
        
        public int TargetValue = 220;

        void Start()
        {
            InitializeDisplay(CurrentValue);
        }

        private void OnValidate()
        {
            if(Application.isPlaying)
                InitializeDisplay(CurrentValue);
        }

        private void InitializeDisplay(int value)
        {
            int temp = value;
            for (int i = 0; i < Segments.Length; i++)
            {
                Segments[i].SetDigitInstant(temp % 10);
                temp /= 10;
            }
        }

        [ContextMenu("Start Countdown")]
        public async void TriggerCountdown()
        {
            await CountdownToValueAsync(TargetValue);
        }

        public async UniTask CountdownToValueAsync(int targetValue)
        {
            while (CurrentValue > targetValue)
            {
                CurrentValue--;
                await RotateAllSegmentsSimultaneously(CurrentValue);
            
                if (DelayBetweenTicksMs > 0)
                    await UniTask.Delay(DelayBetweenTicksMs);
            }
        }




        private async UniTask RotateAllSegmentsSimultaneously(int value)
        {
            int temp = value;
            Sequence multiWheelSequence = DOTween.Sequence();

            for (int i = 0; i < Segments.Length; i++)
            {
                int targetDigit = temp % 10;
                temp /= 10;

                // Only add to sequence if the digit actually changes
                if (Segments[i].CurrentDigit != targetDigit)
                {
                    // Join makes them run at the same time as the previous tween in the sequence
                    multiWheelSequence.Join(Segments[i].GetRotationTween(targetDigit, AnimationDuration));
                }
            }

            // Wait for the entire group of wheels to finish their move
            await multiWheelSequence.ToUniTask();
        }
    }
}