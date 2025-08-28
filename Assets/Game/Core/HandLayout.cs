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
       
        private readonly List<Card> cards = new List<Card>();

        private int selectedIndex = -1;
        private int lastSelectedIndex = -1;
        
        void UpdateLayout()
        {
            int n = cards.Count;
            if (n == 0) return;

            float w = Card.Width;

            if (n == 1)
            {
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

                if (stepFit < Gap) // not enough room to keep gap
                {
                    step = Mathf.Max(w - MinOverlap, 0f); // force overlap
                }
                else
                {
                    step = stepFit;
                }
            }

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

        void Select()
        {
            Debug.Log("Try selecting:"+selectedIndex+" current :"+lastSelectedIndex);
            if (selectedIndex < 0 || selectedIndex >= cards.Count) return;
            var p = cards[selectedIndex].transform.localPosition;
            cards[selectedIndex].transform.localPosition = new Vector3(p.x, p.y+SelectRaiseY, p.z);
            if (lastSelectedIndex != selectedIndex && lastSelectedIndex != -1)
            {
                var p2 = cards[lastSelectedIndex].transform.localPosition;
                cards[lastSelectedIndex].transform.localPosition = new Vector3(p2.x, p2.y - SelectRaiseY, p2.z);
            }

            lastSelectedIndex = selectedIndex;
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
        

        private void AdvanceSelected(int amount)
        {
            if (cards.Count == 0) return;

            if (selectedIndex == -1) selectedIndex = 0;
            else if (selectedIndex <= 0 && amount < 0) selectedIndex = cards.Count - 1;
            else if (selectedIndex >= cards.Count - 1 && amount > 0) selectedIndex = 0;
            else selectedIndex += amount;

            Select();
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
