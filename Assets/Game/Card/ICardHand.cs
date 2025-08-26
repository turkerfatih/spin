namespace Game.Card
{
    public interface ICardHand
    {
        public void AddCard();
        public void DiscardCard(ICard card);
    }
}