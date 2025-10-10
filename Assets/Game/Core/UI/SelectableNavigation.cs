namespace Game.Core.UI
{
    [System.Serializable]
    public class SelectableNavigation
    {
        public ISelectable Up;
        public ISelectable Down;
        public ISelectable Left;
        public ISelectable Right;
    }
}