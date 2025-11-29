using System.Collections.Generic;
using Game.Core;

namespace Game
{
    public interface ICardDatabaseService
    {
        public CardDefinition GetCardData(string cardId);
        public void GetCardsForSelection(List<CardDefinition> cards);
        public CardView CreateCard(CardDefinition cardDefinition);
    }
}