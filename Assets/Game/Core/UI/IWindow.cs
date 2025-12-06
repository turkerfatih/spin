namespace Game.Core.UI
{
    public interface IWindow
    {
        void SetActive(bool isActive);
        void Hide();
        bool HasCustomClose { get; }
    }
}