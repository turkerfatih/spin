using System;
using System.Collections.Generic;
using CardFramework.Deck;
using CardFramework.RandomProvider;
using Game.Actions;
using Game.Core;
using Game.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Game
{
    public class GameSetup:MonoBehaviour,ICardDatabaseService
    {
        public Transform DeckParent;
        public StartingDeck StartingDeck;
        private Deck<Card> playDeck;
        [SerializeField] private CardView CardViewPrefab;
        public CardDatabase Database;
        

        private void Awake()
        {
            Services.GameSetup=this;
            
            CardViewPrefab.gameObject.SetActive(false);
            Services.Actions = new ActionQueue();
            Services.CardsDatabase = this;
            Services.PayTable = new PayTable();
            Services.MainCamera=Camera.main;
            Services.WinLoseCheck = new WinLoseCheckAction();
        }
        

        private void CreateNewGame(ulong seed)
        {
            Services.Random = new PermuteCongruentialGenerator(seed);
            playDeck = new Deck<Card>(Services.Random);
            Services.PlayDeck = playDeck;
            CreateDeck();
            Services.Round = RoundBuilder.GetFirstRound();
            Services.HandSize = 5;
        }

        private void CreateDeck()
        {
            Services.Cards = new List<Card>();
            foreach (var item in StartingDeck.StartingDeckItems)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    var definition = item.Data;
                    var card = new Card(definition);
                    Services.Cards.Add(card);
                    var cardView=Instantiate(CardViewPrefab,Vector3.zero, Quaternion.identity, DeckParent);
                    cardView.Bind(card);
                    card.View=cardView;
                    playDeck.Library.Add(card); 
                }
            }
            playDeck.Library.Shuffle();
            Debug.Log(playDeck.Library.Size);
        }
        
        public void StartNewRun()
        {
            var seedReadable = SeedGenerator.Readable();
            Debug.Log(seedReadable);
            var seedNumeric=SeedGenerator.ToNumeric(seedReadable);
            Debug.Log(seedNumeric);
            CreateNewGame(seedNumeric);
            SceneManager.LoadSceneAsync(Scenes.Round, LoadSceneMode.Single);
            EventBus.OnNewGameLoaded?.Invoke();
        }

        public CardDefinition GetCardData(string cardId)
        {
            foreach (var item in StartingDeck.StartingDeckItems)
            {
                if (string.CompareOrdinal(item.Data.Id, cardId) == 0)
                {
                    return Instantiate(item.Data);
                }
            }
            return null;
        }

        public void GetCardsForSelection(List<CardDefinition> cards)
        {
            var count = Database.AllCards.Count;
            int a = Services.Random.Range(0, count);

            int b;
            do { b = Services.Random.Range(0, count); }
            while (b == a);

            int c;
            do { c = Services.Random.Range(0, count); }
            while (c == a || c == b);
            cards.Add(Database.AllCards[a]);
            cards.Add(Database.AllCards[b]);
            cards.Add(Database.AllCards[c]);
        }

        public CardView CreateCard(CardDefinition cardDefinition)
        {
            var card = new Card(cardDefinition);
            var cardView=Instantiate(CardViewPrefab,Vector3.zero, Quaternion.identity, DeckParent);
            cardView.Bind(card);
            card.View=cardView;
            return cardView;
        }

        public void LoseRun()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}