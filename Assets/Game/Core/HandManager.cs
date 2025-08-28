using System;
using Game.Event;
using UnityEngine;

namespace Game.Core
{
    public class HandManager:MonoBehaviour
    {
        [SerializeField] private int StartingCount;

        private void OnEnable()
        {
            EventBus.OnCardDroppedToSlot += OnCardDroppedToSlot;
        }

        private void OnDisable()
        {
            EventBus.OnCardDroppedToSlot -= OnCardDroppedToSlot;
        }

        private void Start()
        {
            if(!CanDrawCard())
                return;
            for (int i = 0; i < StartingCount; i++)
            {
                if(!CanDrawCard())
                    break;
                var card = Services.Deck.Draw();
                AddCard(card);
            }
        }
        
        

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (CanDrawCard())
                {
                    var card = Services.Deck.Draw();
                    AddCard(card);
                }
            }
        }

        private void AddCard(Card card)
        {
            EventBus.OnCardAddToHand?.Invoke(card);
        }

        private void DiscardCard(Card card)
        {
            card.ResetCard();
            var deck = Services.Deck;
            deck.Discard(card);
        }

        private bool CanDrawCard()
        {
            var deck = Services.Deck;
            if(deck.Library.Size==0 && deck.Discarded.Size==0)
                return false;
            return true;
        }
        

        private void OnCardDroppedToSlot(Card card, int droppedSlot)
        {
            EventBus.OnCardRemovedFromHand?.Invoke(card);
        }
    }
}