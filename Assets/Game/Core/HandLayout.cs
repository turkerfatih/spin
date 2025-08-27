using System;
using System.Collections.Generic;
using Game.Event;
using UnityEngine;

namespace Game.Core
{
    public class HandLayout : MonoBehaviour
    {
        [SerializeField] private float MaxWidth = 8f;
        [SerializeField] private float MinWidth = 2f;
        [SerializeField] private float Gap = 0.2f;
        // How much cards must overlap by (center-to-center < card width) when we decide to overlap
        [SerializeField] private float MinOverlap = 0.02f;

        // Fan settings
        [SerializeField] private float SelectRaiseY = 0.5f;
        [SerializeField] private float PushStrength = 0.4f; // immediate neighbor push
        [SerializeField] private float Falloff = 0.6f;      // push decays per extra neighbor
        private readonly List<Card> cards = new List<Card>();
        private float _step = 0f; // last computed center-to-center spacing
        
        void UpdateLayout()
        {
            int n = cards.Count;
            if (n == 0) return;

            float w = Card.Width;

            if (n == 1)
            {
                _step = 0f;
                cards[0].transform.localPosition = Vector3.zero;
                return;
            }

            // Desired no-overlap step (center-to-center)
            float baseStep = w + Gap;
            float natural = w + (n - 1) * baseStep;

            float step = baseStep;

            if (natural > MaxWidth)
            {
                // Step that would exactly fit within MaxWidth (may be <= w, i.e., overlap)
                float stepFit = (MaxWidth - w) / (n - 1); // n>1 here, so safe

                if (stepFit > w)
                {
                    // This would be a tiny visible gap (< Gap). We prefer overlap instead of "too close".
                    step = Mathf.Max(w - MinOverlap, 0f);
                }
                else
                {
                    // Already overlapping to fit
                    step = Mathf.Max(stepFit, 0f);
                }
            }

            _step = step;

            for (int i = 0; i < n; i++)
            {
                // Start from local x = 0, card i at i * step
                cards[i].transform.localPosition = new Vector3(i * step, 0f, 0f);
            }
        }

        void Select(int index)
        {
            if (index < 0 || index >= cards.Count) return;

            // Ensure we have a valid step if Select is called before layout
            if (cards.Count > 1 && _step <= 0f) UpdateLayout();

            for (int i = 0; i < cards.Count; i++)
            {
                float baseX = i * _step;
                float y = (i == index) ? SelectRaiseY : 0f;

                if (i != index)
                {
                    int d = Mathf.Abs(i - index);          // 1 for immediate neighbor, etc.
                    float offset = PushStrength * Mathf.Pow(Falloff, d - 1);
                    baseX += (i < index) ? -offset : offset;
                }

                cards[i].transform.localPosition = new Vector3(baseX, y, 0f);
            }
        }

        private void OnEnable()
        {
            EventBus.OnCardAddToHand += OnCardAddedToHand;
        }

        private void OnDisable()
        {
            EventBus.OnCardAddToHand -= OnCardAddedToHand;
        }

        private void OnCardAddedToHand(Card card)
        {
            card.gameObject.name="card"+cards.Count.ToString();
            card.transform.parent = transform;
            card.gameObject.SetActive(true);
            AddCard(card);
        }
        

        public void AddCard(Card card)
        {
            cards.Add(card);
            UpdateLayout();
        }

        public void RemoveCard(Card card)
        {
            cards.Remove(card);
            UpdateLayout();
        }
    }
}
