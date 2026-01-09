using System;
using System.Collections.Generic;
using Game.Effect;
using Game.Effects;
using Unity.VisualScripting;


namespace Game.Core
{
    public class Card
    {
        public Guid InstanceId { get; private set; }
        public int Durability;
        public int Charge;
        public string DefinitionId;

        
        [NonSerialized] public CardDefinition Definition;
        [NonSerialized] public ICardView View;
        
        [NonSerialized]
        public Dictionary<CardAttribute, float> PersistentAttributes = new();

        public Card(CardDefinition definition)
        {
            InstanceId = Guid.NewGuid();
            Definition =  definition;
            DefinitionId =  definition.Id;
            Durability = definition.Durability;
            Charge = definition.Charge;
        }

        public bool OnAfterSpin()
        {
            Durability--;
            View?.UpdateDurability();
            return Durability <= 0;
        }

        public void Reset()
        {
            Durability = Definition.Durability;
        }
        public void SetAttribute(CardAttribute key, float value)
        {
            PersistentAttributes[key] = value;
        }

        public float GetAttribute(CardAttribute key, float defaultValue = 0)
        {
            return PersistentAttributes.TryGetValue(key, out var v) ? v : defaultValue;
        }

        public void AddToAttribute(CardAttribute key, float amount)
        {
            PersistentAttributes[key] = GetAttribute(key) + amount;
        }
    }
}