using Game.Core;

namespace Game
{
    public interface ICardDatabaseService
    {
        public CardDefinition GetCardData(string cardId);
    }
}