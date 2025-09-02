namespace Game.Core
{
    public interface ICardView
    {
        public void Bind(Card card);
        public void UnBind();
        
        public void UpdateDurability();

    }
}