using System;
using CardFramework.Deck;
using CardFramework.RandomProvider;
using Game.Core;
using Game.Event;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameSetup:MonoBehaviour,ICardDatabaseService
    {
        public Transform DeckParent;
        public StartingDeck StartingDeck;
        public Card CardPrefab;
        public Deck<Card> Deck;
        
        public CardDatabase CardDatabase;

        private void Awake()
        {
            Services.Cards = this;
            Services.PayTable = new PayTable();
        }

        private void Start()
        {
            StartNewRun();
        }

        private void CreateNewGame(ulong seed)
        {

            Services.Random = new PermuteCongruentialGenerator(seed);
            Deck = new Deck<Card>(Services.Random);
            Services.Deck = Deck;
            CreateDeck();
        }

        private void CreateDeck()
        {
            foreach (var item in StartingDeck.StartingDeckItems)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    var card=Instantiate(CardPrefab,Vector3.zero, Quaternion.identity, DeckParent);
                    card.Load(item.Data);
                    Deck.Library.Add(card); 
                }
            }
            Deck.Library.Shuffle();
            Debug.Log(Deck.Library.Size);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
            }
        }

        public void StartNewRun()
        {
            var seedReadable = SeedGenerator.Readable();
            Debug.Log(seedReadable);
            var seedNumeric=SeedGenerator.ToNumeric(seedReadable);
            Debug.Log(seedNumeric);
            CreateNewGame(seedNumeric);
            //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            EventBus.OnNewGameLoaded?.Invoke();
        }
        public void ContinueLastRun()
        {
            
        }

        //provide card data from starting deck just for now but later we can
        //create sperate service and database of card infos as scriptable object?!  
        public CardData GetCardData(string cardId)
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
    }
}