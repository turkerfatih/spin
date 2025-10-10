namespace Game.Core.UI
{
    public interface ISelectable
    {
        void OnSelected();
        void OnDeselected();
        void OnSubmit();
        SelectableNavigation Navigation { get; }
    }
}