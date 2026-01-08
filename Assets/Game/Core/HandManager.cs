using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Event;
using Game.Pooling;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class HandManager:MonoBehaviour
    {
        [SerializeField] private bool DrawAll;
        
        
        private List<Card> hand = new List<Card>();

        public IHandView View { get;  set; }

        [NonSerialized]
        public Transform DeckParent;
        

        private void OnEnable()
        {
            DeckParent = Services.GameSetup.DeckParent;
            EventBus.OnCardDroppedToSlot += OnCardDroppedToSlot;
        }

        private void OnDisable()
        {
            EventBus.OnCardDroppedToSlot -= OnCardDroppedToSlot;
        }

        public void StartDraw()
        {
            DrawNewHand().Forget();
        }

        private async UniTask DrawNewHand()
        {
            if (!CanDrawCard())
            {
                Debug.LogWarning("Cant draw card");
                return;
            }
            Services.PlayDeck.Library.Shuffle();
            var count = Services.HandSize; //StartingCount;
            if(DrawAll)
            {
                count = Services.PlayDeck.Library.Size;
            }
            for (int i = 0; i < count; i++)
            {
                if (!CanDrawCard())
                {
                    break;
                }

                var card = Services.PlayDeck.Draw();
                EventBus.OnDrawPileChanged?.Invoke(Services.PlayDeck.Library.Size);
                EventBus.OnDiscardPileChanged?.Invoke(Services.PlayDeck.Discarded.Size);
                AddCard(card);
            }
        }

        public async UniTask PostSpinAction()
        {
            for (var i = hand.Count - 1; i >= 0; i--)
            {
                var card = hand[i];
                DiscardCard(card);
            }

            await DrawNewHand();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                /*if (CanDrawCard())
                {
                    var card = Services.Deck.Draw();
                    EventBus.OnDrawPileChanged?.Invoke(Services.Deck.Library.Size);
                    AddCard(card);
                }*/
                Services.Machine.PrintCurrentSymbols();
            }
        }

        private void AddCard(Card card)
        {
            hand.Add(card);
            var cardView=Services.Pool.Get<CardView>();
            cardView.Bind(card);
            card.View=cardView;
            View.AddCard(card);
            card.View.DrawAnimation();
        }
        
        private void DiscardCard(Card card,bool fromSlot=false)
        {
            Debug.Log("Card discarded from slot:"+fromSlot);
            var deck = Services.PlayDeck;
            deck.Discard(card);
            EventBus.OnDiscardPileChanged?.Invoke(deck.Discarded.Size);
            if (!fromSlot)
            {
                hand.Remove(card);
                View.DiscardCard(card);
                card.View.DiscardAnimation();
            }
        }

        private bool CanDrawCard()
        {
            var deck = Services.PlayDeck;
            if(deck.Library.Size==0 && deck.Discarded.Size==0)
                return false;
            return true;
        }
        

        private void OnCardDroppedToSlot(CardView cardView, int droppedSlot)
        {
            var slot= Services.DropSlot.GetDropSlot(droppedSlot);
            slot.SetCard(cardView);
            hand.Remove(cardView.Model);
            View.RemoveCard(cardView.Model);
        }

        public void OnCardReturnFromSlot(Card card)
        {
            DiscardCard(card,fromSlot:true);
            card.Reset();
            //AddCard(card);
        }
        public int NumberOfCardsInHand=> hand.Count;
        
        public void RoundFinished()
        {
            Services.PlayDeck.ReturnDiscardsToBottomOfLibrary();
            Services.PlayDeck.ReturnExiledToBottomOfLibrary();
        }
    }
}