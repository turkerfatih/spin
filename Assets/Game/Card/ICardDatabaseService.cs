using Game.Core;

namespace Game
{
    public interface ICardDatabaseService
    {
        public CardData GetCardData(string cardId);
    }
}