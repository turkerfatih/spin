using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Event;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class HandManager:MonoBehaviour
    {
        [SerializeField] private int StartingCount;
        [SerializeField] private bool DrawAll;
        
        
        private List<Card> hand = new List<Card>();

        public IHandView View { get;  set; }
        

        private void Awake()
        {
            Services.Hand = this;
        }

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
            DrawNewHand().Forget();
        }

        private async UniTask DrawNewHand()
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
                EventBus.OnDrawPileChanged?.Invoke(Services.Deck.Library.Size);
                EventBus.OnDiscardPileChanged?.Invoke(Services.Deck.Discarded.Size);
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
            View.AddCard(card);
            card.View.DrawAnimation();
        }
        
        private void DiscardCard(Card card,bool fromSlot=false)
        {
            Debug.Log("Card discarded from slot:"+fromSlot);
            var deck = Services.Deck;
            deck.Discard(card);
            EventBus.OnDiscardPileChanged?.Invoke(deck.Discarded.Size);
            if (!fromSlot)
            {
                hand.Remove(card);
                View.DiscardCard(card);
            }
            card.View.DiscardAnimation();
        }

        private bool CanDrawCard()
        {
            var deck = Services.Deck;
            if(deck.Library.Size==0 && deck.Discarded.Size==0)
                return false;
            return true;
        }
        

        private void OnCardDroppedToSlot(CardView cardView, int droppedSlot)
        {
            var slot= Services.Machine.GetDropSlot(droppedSlot);
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
    }
}