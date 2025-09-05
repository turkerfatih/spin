namespace Game.Core
{
    public interface IHandView
    {
        void AddCard(Card card);
        void RemoveCard(Card card);
        void DiscardCard(Card card);
    }
}