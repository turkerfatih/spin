using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Event;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class ReelView : MonoBehaviour
    {
        public const float Width = 2f;
        private const float SpinDuration = 1.5f;      // total spin time (ease-in + ease-out)
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

            float currentY = VerticalList.localPosition.y;

            // target position in reel space
            float targetY = SymbolView.Height * targetIndex;

            // move downward until we reach the target (add extra laps)
            float distance = targetY - currentY;
            
            while (distance > 0) distance -= bottomLimit;
                distance -= bottomLimit * 1; // at least 1 laps for animation feel

            float finalY = currentY + distance;

            // tween with ease-in-out curve
            await DOVirtual.Float(0f, 1f, SpinDuration, t =>
            {
                float eased = EaseInOutCubic(t);
                float val = Mathf.Lerp(currentY, finalY, eased);

                // wrap downward safely
                float wrapped = val % bottomLimit;
                if (wrapped < 0) wrapped += bottomLimit;

                VerticalList.localPosition = new Vector3(0, wrapped, 0);
            })
            .ToUniTask();

            // 🔒 snap-to-grid (pixel-perfect)
            float snapped = Mathf.Round(VerticalList.localPosition.y / SymbolView.Height) * SymbolView.Height;
            snapped = snapped % bottomLimit;
            if (snapped < 0) snapped += bottomLimit;

            VerticalList.localPosition = new Vector3(0, snapped, 0);

            OnSpinComplete();
        }

        private float EaseInOutCubic(float t)
        {
            t = Mathf.Clamp01(t);
            return t < 0.5f 
                ? 1f * t * t * t 
                : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
        }

        private void OnSpinComplete()
        {
            EventBus.OnReelSpinEnd?.Invoke(reelIndex);
        }

        // Optional Push/Pull nudges
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
