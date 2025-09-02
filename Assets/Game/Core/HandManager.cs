using Game.Event;
using UnityEngine;

namespace Game.Core
{
    public class HandManager:MonoBehaviour
    {
        [SerializeField] private int StartingCount;
        [SerializeField] private bool DrawAll;
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
            var count = StartingCount;
            if(DrawAll)
            {
                count = Services.Deck.Library.Size;
            }
            for (int i = 0; i < count; i++)
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
            
            EventBus.OnCardAddToHand?.Invoke(card.View as CardView);
        }
        
        private void DiscardCard(Card card)
        {
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
        

        private void OnCardDroppedToSlot(CardView card, int droppedSlot)
        {
            var slot= Services.Machine.GetDropSlot(droppedSlot);
            slot.SetCard(card);
            EventBus.OnCardRemovedFromHand?.Invoke(card);
        }

        public void OnCardReturnFromSlot(Card card)
        {
            DiscardCard(card);
            card.Reset();
            //AddCard(card);
        }
    }
}