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

        private readonly bool[] moving = { false, false, false };

        void Start()
        {
            InitializeDisplay(currentValue);
        }

        public UniTask Setup(int startValue)
        {
            currentValue = 0;
            InitializeDisplay(currentValue);
            targetValue=startValue;
            return DoMechanicalJump(targetValue, 1.5f); 
        }

        public UniTask Reduce(int amount)
        {
            targetValue= currentValue - amount;
            return DoMechanicalJump(targetValue, 1.5f);
        }

        private void OnValidate()
        {
            if(Application.isPlaying)
                InitializeDisplay(currentValue);
        }

        private void InitializeDisplay(int value)
        {
            var temp = value;
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
            Sequence parallelSequence = DOTween.Sequence();

            for (int i = 0; i < numbers.Length; i++)
            {
                int targetDigit = temp % 10;
                temp /= 10;

                // Only add to sequence if the digit actually changes
                if (numbers[i].CurrentDigit != targetDigit)
                {
                    // Join makes them run at the same time as the previous tween in the sequence
                    parallelSequence.Join(numbers[i].GetRotationTween(targetDigit, animationDuration));
                }
            }

            // Wait for the entire group of wheels to finish their move
            await parallelSequence.ToUniTask();
        }
        
        
        [ContextMenu("Mechanical Jump")]
        public async void MechanicalJump()
        {
            // Jump from 249 to 000 (or any target)
            await DoMechanicalJump(targetValue, 1.5f);
        }

        public async UniTask DoMechanicalJump(int target, float duration)
        {
            
            //UpdateMovingArray(currentValue,targetValue);
            var change = currentValue - targetValue;
            Debug.Log($"{currentValue} - {change}={currentValue-change}");
            SubtractAndUpdateMoving(currentValue, change);
            UpdateMovingForAnimation();
            Debug.Log($"{moving[2]} {moving[1]} {moving[0]}");
            Sequence s = DOTween.Sequence();
            int tempTarget = target;

            for (int i = 0; i < numbers.Length; i++)
            {
                int targetDigit = tempTarget % 10;
                tempTarget /= 10;
                var t = numbers[i].GetRotationTween(targetDigit, duration, moving[i]);
                t.SetDelay(i * 0.25f);
                s.Join(t);
            }

            currentValue = targetValue;
            await s.ToUniTask();
        }

        private void UpdateMovingForAnimation()
        {
            for (int i = moving.Length-1; i > 0; i--)
            {
                if (moving[i])
                {
                    for (int j = i-1; j >= 0; j--)
                    {
                        moving[j] = true;
                    }
                    break;
                }
            }
        }

        public  int SubtractAndUpdateMoving(
            int originalNumber,
            int subtractValue)
        {
            int result = 0;
            int place = 1;
            int index = 0;

            while (originalNumber > 0 || subtractValue > 0)
            {
                int originalDigit = originalNumber % 10;
                int subtractDigit = subtractValue % 10;

                int resultDigit = originalDigit - subtractDigit;

                // Handle borrow
                if (resultDigit < 0)
                {
                    resultDigit += 10;
                    subtractValue += 10; // propagate borrow
                }

                // Overwrite moving state (ignore initial values)
                if (index < moving.Length)
                    moving[index] = (resultDigit != originalDigit);

                result += resultDigit * place;

                place *= 10;
                originalNumber /= 10;
                subtractValue /= 10;
                index++;
            }
            
            // Clear remaining digits if moving is longer
            for (; index < moving.Length; index++)
                moving[index] = false;

            return result;
        }
    }
}