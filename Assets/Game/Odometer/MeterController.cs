using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Serialization;

namespace Game.Odometer
{
    public class MeterController:MonoBehaviour
    {
        [SerializeField] private MeterNumber[] numbers; // 0: Units, 1: Tens, 2: Hundreds
        [SerializeField] private int currentValue = 249;
        [SerializeField] private int targetValue = 238;
        
    
        [Header("Settings")]
        [SerializeField] private float animationDuration = 0.4f; // Total time for all wheels to finish
        [SerializeField] private int delayBetweenTicksMs = 100;
        

        void Start()
        {
            InitializeDisplay(currentValue);
        }

        private void OnValidate()
        {
            if(Application.isPlaying)
                InitializeDisplay(currentValue);
        }

        private void InitializeDisplay(int value)
        {
            int temp = value;
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i].SetDigitInstant(temp % 10);
                temp /= 10;
            }
        }

        [ContextMenu("Start Countdown")]
        public async void TriggerCountdown()
        {
            await CountdownToValueAsync(targetValue);
        }

        public async UniTask CountdownToValueAsync(int value)
        {
            while (currentValue > value)
            {
                currentValue--;
                await RotateAllSegmentsSimultaneously(currentValue);
            
                if (delayBetweenTicksMs > 0)
                    await UniTask.Delay(delayBetweenTicksMs);
            }
        }




        private async UniTask RotateAllSegmentsSimultaneously(int value)
        {
            int temp = value;
            Sequence multiWheelSequence = DOTween.Sequence();

            for (int i = 0; i < numbers.Length; i++)
            {
                int targetDigit = temp % 10;
                temp /= 10;

                // Only add to sequence if the digit actually changes
                if (numbers[i].CurrentDigit != targetDigit)
                {
                    // Join makes them run at the same time as the previous tween in the sequence
                    multiWheelSequence.Join(numbers[i].GetRotationTween(targetDigit, animationDuration));
                }
            }

            // Wait for the entire group of wheels to finish their move
            await multiWheelSequence.ToUniTask();
        }
    }
}