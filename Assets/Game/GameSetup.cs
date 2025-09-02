using System;
using CardFramework.Deck;
using CardFramework.RandomProvider;
using Game.Actions;
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
        public Deck<Card> Deck;
        [SerializeField] private CardView CardViewPrefab;
        
        public CardDatabase CardDatabase;

        private void Awake()
        {
            CardViewPrefab.gameObject.SetActive(false);
            Services.Actions = new ActionQueue();
            Services.Cards = this;
            Services.PayTable = new PayTable();
            Services.MainCamera=Camera.main;
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
                    var definition = item.Data;
                    var card = new Card(definition);
                    var cardView=Instantiate(CardViewPrefab,Vector3.zero, Quaternion.identity, DeckParent);
                    cardView.Bind(card);
                    card.View=cardView;
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
    }
}