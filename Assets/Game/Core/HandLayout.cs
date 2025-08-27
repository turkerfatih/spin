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

            // Desired step without overlap
            float baseStep = w + Gap;
            float natural = w + (n - 1) * baseStep;

            float step = baseStep;

            if (natural > MaxWidth)
            {
                // Step that would exactly fit in MaxWidth
                float stepFit = (MaxWidth - w) / (n - 1);

                if (stepFit > w)
                {
                    // If spacing is smaller than Gap but not overlapping, force overlap
                    step = Mathf.Max(w - MinOverlap, 0f);
                }
                else
                {
                    step = Mathf.Max(stepFit, 0f);
                }
            }

            _step = step;

            // Total occupied width
            float totalWidth = w + (n - 1) * step;
            // Leftmost card position so the hand is centered
            float startX = -totalWidth / 2f + w / 2f;

            for (int i = 0; i < n; i++)
            {
                float x = startX + i * step;
                cards[i].transform.localPosition = new Vector3(x, 0f, 0f);
            }
        }

        void Select(int index)
        {
            Debug.Log("Selected:"+index);
            if (index < 0 || index >= cards.Count) return;
            if (cards.Count > 1 && _step <= 0f) UpdateLayout();

            float w = Card.Width;
            int n = cards.Count;

            float totalWidth = w + (n - 1) * _step;
            float startX = -totalWidth / 2f + w / 2f;

            for (int i = 0; i < n; i++)
            {
                float x = startX + i * _step;
                float y = (i == index) ? SelectRaiseY : 0f;

                if (i != index)
                {
                    int d = Mathf.Abs(i - index);
                    float offset = PushStrength * Mathf.Pow(Falloff, d - 1);
                    x += (i < index) ? -offset : offset;
                }

                cards[i].transform.localPosition = new Vector3(x, y, 0f);
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

        private void OnMouseDown()
        {
            
        }
        private int selectedIndex = -1;

        private void AdvanceSelected(int amount)
        {
            
            if (selectedIndex < 0 || amount < 0)
            {
                selectedIndex = 1;
            }

            selectedIndex += amount;
            selectedIndex = selectedIndex % cards.Count;
            Select(selectedIndex);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                AdvanceSelected(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                AdvanceSelected(1);
            }
        }
    }
}
