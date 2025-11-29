using System;
using System.Collections.Generic;
using Game.Core;
using Game.Core.UI;
using UnityEngine;

namespace Game.UI
{
    public class CardSelection:MonoBehaviour
    {
        [SerializeField]
        private List<Transform> CardHolders;
        
        private List<CardDefinition> cardDefinitions = new List<CardDefinition>();
        private List<CardView> cardViews = new List<CardView>();

        private void Awake()
        {
            Services.CardSelection = this;
        }

        public void Show()
        {
            Debug.Log("Card selection Show");
            Services.CardsDatabase.GetCardsForSelection(cardDefinitions);
            for (var i = 0; i < cardDefinitions.Count; i++)
            {
                var cardDefinition = cardDefinitions[i];
                var card = Services.CardsDatabase.CreateCard(cardDefinition);
                cardViews.Add(card);
                card.gameObject.transform.SetParent(CardHolders[i].transform, false);
                card.SetSortingLayer(LayerHelper.Popup);
                card.SetOrder(5);
                card.gameObject.SetActive(true);
            }
            if (!UIOverlay.Instance.IsOpen)
            {
                UIOverlay.Instance.Show();
            }
            gameObject.SetActive(true);
        }

    }
}