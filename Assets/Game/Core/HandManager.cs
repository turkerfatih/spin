using System;
using Game.Event;
using UnityEngine;

namespace Game.Core
{
    public class HandManager:MonoBehaviour
    {
        [SerializeField] private int StartingCount;
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

        private void AddCard(Card card)
        {
            EventBus.OnCardAddToHand?.Invoke(card);
        }

        private bool CanDrawCard()
        {
            var deck = Services.Deck;
            if(deck.Library.Size==0 && deck.Discarded.Size==0)
                return false;
            return true;
        }
    }
}