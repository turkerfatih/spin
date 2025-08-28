using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Event;
using UnityEngine;

namespace Game.Core
{
    public class HandLayout : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer Box;
        [SerializeField] private float MaxWidth = 8f;
        [SerializeField] private float MinWidth = 2f;
        [SerializeField] private float Gap = 0.2f;
        // How much cards must overlap by (center-to-center < card width) when we decide to overlap
        [SerializeField] private float MinOverlap = 0.02f;

        // Fan settings
        [SerializeField] private float SelectRaiseY = 0.5f;
       
        private readonly List<Card> cards = new List<Card>();
        private readonly List<Vector3> positions = new List<Vector3>();

        private int selectedIndex = -1;
        private int lastSelectedIndex = -1;
        
        private BoxCollider boxCollider;
        private float currentTotalWidth;
        
        private bool isDragging = false;
        [SerializeField] private CardDragger Dragger;
        
        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
            currentTotalWidth = MinWidth;
        }

        void UpdateLayout()
        {
            CalculatePositions();
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].transform.localPosition = positions[i];
                cards[i].SetOrder(i);
            }
        }

        void CalculatePositions()
        {
            int n = cards.Count;
            if (n == 0) return;

            float w = Card.Width;

            if (n == 1)
            {
                positions[0] = Vector3.zero;
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

                if (stepFit <= w) // not enough room even to touch
                {
                    step = Mathf.Max(w - MinOverlap, 0f); // force overlap
                }
                else if (stepFit < baseStep) // narrower than natural spacing
                {
                    step = stepFit;
                }
                else
                {
                    step = baseStep;
                }
            }

            // Total occupied width
            float totalWidth = w + (n - 1) * step;
            currentTotalWidth=totalWidth;
            // Leftmost card position so the hand is centered
            float startX = -totalWidth / 2f + w / 2f;

            for (int i = 0; i < n; i++)
            {
                float x = startX + i * step;
                positions[i] = new Vector3(x, 0f, 0f);
            }
            UpdateBox();
        }

        private void UpdateBox()
        {
            var width = Mathf.Max(currentTotalWidth, MinWidth)+Card.Width;            
            boxCollider.size = new Vector3(width, boxCollider.size.y, boxCollider.size.z);
            boxCollider.center = new Vector3(0f, boxCollider.center.y, boxCollider.center.z);
            var bs = Box.size;

            Box.size = new Vector2(width  , bs.y);
        }

        void Select()
        {
            Debug.Log("Try selecting:"+selectedIndex+" current :"+lastSelectedIndex);
            if(selectedIndex==lastSelectedIndex)
                return;
            if (selectedIndex < 0 || selectedIndex >= cards.Count) return;
            RemoveCurrentlySelected();
            SetCardSelected();
            lastSelectedIndex = selectedIndex;
        }

        private void RemoveCurrentlySelected()
        {
            if ( lastSelectedIndex == -1) return;
            var target= positions[lastSelectedIndex];
            cards[lastSelectedIndex].transform.DOLocalMove(target,0.15f).SetEase(Ease.OutExpo);
        }

        private void SetCardSelected()
        {
            var p = positions[selectedIndex];
            var target = new Vector3(p.x, p.y+SelectRaiseY, p.z);
            cards[selectedIndex].transform.DOLocalMove(target,0.15f).SetEase(Ease.OutQuint); 
        }

        private void OnEnable()
        {
            EventBus.OnCardAddToHand += OnCardAddedToHand;
            EventBus.OnDragCancel += OnDraggingCancel;
            EventBus.OnCardRemovedFromHand+= OnCardRemovedFromHand;
        }

        private void OnDisable()
        {
            EventBus.OnCardAddToHand -= OnCardAddedToHand;
            EventBus.OnDragCancel -= OnDraggingCancel;
            EventBus.OnCardRemovedFromHand-= OnCardRemovedFromHand;
        }

        private void OnCardRemovedFromHand(Card card)
        {
            RemoveCard(card);
            ResetValues();
        }

        private void OnCardAddedToHand(Card card)
        {
            card.gameObject.name="card"+cards.Count.ToString();
            card.transform.parent = transform;
            card.gameObject.SetActive(true);
            AddCard(card);
        }


        private void AddCard(Card card)
        {
            cards.Add(card);
            positions.Add(new Vector3());
            card.ChangeId(cards.Count);
            UpdateLayout();
        }

        private void RemoveCard(Card card)
        {
            var index = cards.IndexOf(card);
            cards.RemoveAt(index);
            positions.RemoveAt(index);
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
            if(isDragging)
                return;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                AdvanceSelected(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                AdvanceSelected(1);
            }
            
        }

        private void OnMouseOver()
        {
            if(isDragging)return;
            if (cards.Count == 0) return;
            Vector3 worldMouse = Services.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 localMouse = transform.InverseTransformPoint(worldMouse);
            float width = boxCollider.size.x;
            float halfWidth = width / 2f;
            float x = Mathf.Clamp(localMouse.x, -halfWidth, halfWidth);
            float t = (x + halfWidth) / width;
            int index = Mathf.FloorToInt(t * cards.Count);
            index = Mathf.Clamp(index, 0, cards.Count - 1);
            if (index != selectedIndex)
            {
                selectedIndex = index;
                Select();
            }
        }

        private void OnMouseDown()
        {
            if(isDragging)return;
            if(lastSelectedIndex==-1) return;
            DragCard();
        }

        private void OnMouseExit()
        {
            if(isDragging)return;
            RemoveFocus();
        }

        private void RemoveFocus()
        {
            RemoveCurrentlySelected();
            ResetValues();
        }

        private void ResetValues()
        {
            selectedIndex = -1;
            lastSelectedIndex = -1;
            isDragging = false;
        }

        private void DragCard()
        {
            isDragging = true;
            Dragger.StartDragging(cards[lastSelectedIndex]);
        }

        private void OnDraggingCancel(Card card)
        {
            var index=cards.IndexOf(card);
            cards[index].transform.DOLocalMove(positions[index], 0.15f)
                .SetEase(Ease.InOutCubic).OnComplete(() =>
                {
                    isDragging = false;
                    RemoveFocus();
                    card.SetOrder(index);
                });
        }
    }
}
