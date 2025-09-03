using System;
using System.Collections.Generic;
using Game.Effect;
using Game.Effects;
using Unity.VisualScripting;


namespace Game.Core
{
    public class Card
    {
        public Guid Id { get; } = Guid.NewGuid();
        public int Durability;
        public CardDefinition Definition { get; }
        public ICardView View { get; set; }

        public Card(CardDefinition definition)
        {
            
            Definition =  definition;
            Durability = definition.Durability;
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
    }
}