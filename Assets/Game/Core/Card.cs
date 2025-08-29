using System;
using System.Collections.Generic;
using Game.Effect;


namespace Game.Core
{
    public class Card
    {
        public Guid Id { get; } = Guid.NewGuid();
        public CardDefinition Definition { get; }
        public List<CardEffect> Effects { get; } = new();

        public Card(CardDefinition definition)
        {
            Definition = definition;
            foreach (var e in definition.Effects)
                Effects.Add(e.Clone());
        }
    }
}