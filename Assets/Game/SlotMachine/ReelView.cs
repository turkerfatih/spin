using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using Game.Event;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class ReelView : MonoBehaviour
    {
        public const float Width = 2f;
        private const float BaseSpeed = SymbolView.Height * 20f; // base spin speed
        private const float SpinDuration = 0.5f;                 // how long before decel starts
        private const float DecelDuration = 0.5f;                // slowdown duration
        private const int DuplicatesNeeded = 3;

        [SerializeField]
        private Transform VerticalList;

        private List<Symbol> symbols;
        private List<SymbolView> views;
        private int reelIndex;
        private float bottomLimit;

        public void Setup(List<Symbol> items, int index)
        {
            reelIndex = index;
            symbols = items;
            views = new List<SymbolView>(items.Count);
            bottomLimit = items.Count * SymbolView.Height;

            var pos = new Vector3(0, SymbolView.Height * DuplicatesNeeded, 0);

            // top duplicates
            for (int i = 0; i < DuplicatesNeeded; i++)
            {
                AddView(items.Count - DuplicatesNeeded + i, pos);
                pos.y -= SymbolView.Height;
            }

            // main sequence
            for (int i = 0; i < symbols.Count; i++)
            {
                AddView(i, pos);
                pos.y -= SymbolView.Height;
            }

            // bottom duplicates
            for (int i = 0; i < DuplicatesNeeded; i++)
            {
                AddView(i, pos);
                pos.y -= SymbolView.Height;
            }
        }

        private void AddView(int index, Vector3 pos)
        {
            var sym = symbols[index];
            var symbolView = Services.SymbolBuilder.GetView(VerticalList, sym, index);
            views.Add(symbolView);
            symbolView.transform.localPosition = pos;
        }

        public async UniTask Spin(Guid symbolId, float delay = 0f)
        {
            int targetIndex = symbols.FindIndex(s => s.Id == symbolId);
            if (targetIndex == -1)
            {
                Debug.LogWarning("Symbol ID not found on reel.");
                return;
            }

            await Spin(targetIndex, delay);
        }

        private async UniTask Spin(int targetIndex, float delay = 0f)
        {
            // random stagger so reels don’t start perfectly together
            var startDelay = Random.Range(0f, 0.15f) + delay;
            await UniTask.WaitForSeconds(startDelay);

            float time = 0f;
            float speed = BaseSpeed;

            // --- SPIN PHASE (constant speed) ---
            while (time < SpinDuration)
            {
                StepReel(ref speed);
                time += Time.deltaTime;
                await UniTask.WaitForEndOfFrame(this);
            }

            // --- DECELERATION PHASE ---
            float currentY = VerticalList.localPosition.y;
            float targetY = SymbolView.Height * targetIndex;

            // compute distance to target (always downward / negative direction)
            float distance = targetY - currentY;
            while (distance > 0) distance -= bottomLimit; // ensure negative
            distance -= bottomLimit; // force at least one extra lap

            float finalY = currentY + distance;

            // Tween "progress" instead of raw position to avoid direction flips
            await DOVirtual.Float(0f, 1f, DecelDuration, t =>
            {
                float eased = EaseOutCubic(t);   
                float val = currentY + distance * eased;

                // wrap downward safely
                float wrapped = val % bottomLimit;
                if (wrapped < 0) wrapped += bottomLimit;

                VerticalList.localPosition = new Vector3(0, wrapped, 0);
            }).ToUniTask();

            OnSpinComplete();
        }
        
        float EaseOutCubic(float t) {
            t = Mathf.Clamp01(t);
            t = t - 1f;
            return t * t * t + 1f;
        }

        private void StepReel(ref float speed)
        {
            float currentY = VerticalList.localPosition.y;
            currentY -= speed * Time.deltaTime;

            // manual downward wrap
            if (currentY < 0)
                currentY += bottomLimit;

            VerticalList.localPosition = new Vector3(0, currentY, 0);
        }

        private void OnSpinComplete()
        {
            EventBus.OnReelSpinEnd?.Invoke(reelIndex);
        }

        // --- Optional Push/Pull nudges ---
        public void Push() => PushOrPull(1);
        public void Pull() => PushOrPull(-1);

        private void PushOrPull(int dir)
        {
            float amount = SymbolView.Height * dir;
            float currentY = VerticalList.localPosition.y;
            float targetY = currentY + amount;

            // wrap downward
            targetY = targetY % bottomLimit;
            if (targetY < 0) targetY += bottomLimit;

            VerticalList.DOLocalMoveY(targetY, 0.75f)
                .SetEase(dir > 0 ? Ease.InBack : Ease.OutBack)
                .OnComplete(OnSpinComplete);
        }
    }
}
