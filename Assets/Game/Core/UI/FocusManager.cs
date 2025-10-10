using UnityEngine;

namespace Game.Core.UI
{
    public class FocusManager:MonoBehaviour
    {
        public static FocusManager Instance { get; private set; }
        public ISelectable Current { get; private set; }
        void Awake() => Instance = this;
        public void SetFocus(ISelectable target)
        {
            if (Current == target) return;
            Current?.OnDeselected();
            Current = target;
            Current?.OnSelected();
        }
        public void Navigate(Vector2 dir)
        {
            if (Current == null) return;
            var nav = Current.Navigation;

            ISelectable next = null;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                next = dir.x > 0 ? nav.Right : nav.Left;
            else
                next = dir.y > 0 ? nav.Up : nav.Down;

            if (next != null)
                SetFocus(next);
        }
    }
}