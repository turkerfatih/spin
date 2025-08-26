namespace Game.Core
{
    public interface ICardHand
    {
        public void AddCard();
        public void DiscardCard(Card card);
    }
}