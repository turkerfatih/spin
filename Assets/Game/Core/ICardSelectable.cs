namespace Game.Core
{
    public interface ICardSelectable
    {
        public ICardSelectionHandler SelectionHandler { get; set; }
        public void Focus();
    }
}