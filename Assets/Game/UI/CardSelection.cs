using System;
using System.Collections.Generic;
using Game.Core;
using Game.Core.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    public class CardSelection:MonoBehaviour, ICardSelectionHandler
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
            cardViews.Clear();
            for (var i = 0; i < cardDefinitions.Count; i++)
            {
                var cardDefinition = cardDefinitions[i];
                var cardView = Services.CardsDatabase.CreateCard(cardDefinition);
                cardViews.Add(cardView);
                cardView.gameObject.transform.SetParent(CardHolders[i].transform, false);
                cardView.SetSortingLayer(SortLayerHelper.Popup);
                cardView.SetOrder(5);
                cardView.gameObject.SetActive(true);
                var button = cardView.GetComponent<CardButton>();
                button.gameObject.layer = LayerHelper.UI;
                button.Index = i;
                button.View = cardView;
                button.SelectionHandler = this;
            }
            if (!UIOverlay.Instance.IsOpen)
            {
                UIOverlay.Instance.Show();
            }
            gameObject.SetActive(true);
        }

        public void OnCardSelected(CardButton button)
        {
            Debug.Log($"Selection Index {button.Index}");
            Debug.Log("add card to deck and close gameplay probably save game state");
            Services.Cards.Add(button.View.Model);
            //todo: where to proceed, lets proceed to decision scene
        }
    }
}