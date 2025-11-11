using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core
{
    public class CardFactory
    {
        private static Dictionary<string, CardDefinition> definitions;

        public static void Initialize(IEnumerable<CardDefinition> allDefinitions)
        {
            definitions = allDefinitions.ToDictionary(d => d.Id, d => d);
        }
        
        public static CardSaveData ToSaveData(Card card)
        {
            return new CardSaveData
            {
                DefinitionId = card.DefinitionId,
                InstanceId = card.InstanceId,
                Durability = card.Durability
            };
        }
        
        public static Card CreateFromSave(CardSaveData save)
        {
            if (!definitions.TryGetValue(save.DefinitionId, out var def))
            {
                Debug.LogError($"CardDefinition with ID {save.DefinitionId} not found!");
                return null;
            }

            var card = new Card(def)
            {
                // override runtime values from save
                Durability = save.Durability
            };
            foreach (var attr in save.Attributes)
                card.PersistentAttributes[attr.Key] = attr.Value;

            return card;
        }

        public static List<Card> LoadFromSave()
        {
            return null;
        }

        public static void PrepareCardSave(RunData runData,List<Card> cards)
        {
            if(runData.Cards==null)
                runData.Cards = new List<CardSaveData>();
            else
            {
                runData.Cards.Clear();
            }
            foreach (var card in cards)
            {
                var cardSaveData = ToSaveData(card);
                foreach (var attr in card.PersistentAttributes)
                {
                    var a=new CardAttributeSaveData
                    {
                        Key = attr.Key,
                        Value = attr.Value
                    };
                    cardSaveData.Attributes.Add(a);
                }

                runData.Cards.Add(cardSaveData);
            }
        }

        public static void LoadSavedCards(RunData runData)
        {
            if(runData.RuntimeCards==null)
                runData.RuntimeCards = new List<Card>();
            else
            {
                runData.RuntimeCards.Clear();
            }
            foreach (var card in runData.Cards)
            {
                runData.RuntimeCards.Add(CreateFromSave(card));
            }
        }
    }
}