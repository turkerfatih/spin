using UnityEngine;

namespace Game.Core.UI
{
    public class SelectableComponent : MonoBehaviour, ISelectable
    {
        [SerializeField] private SelectableNavigation navigation = new();

        public SelectableNavigation Navigation => navigation;

        public virtual void OnSelected() { /* highlight visuals */ }
        public virtual void OnDeselected() { /* unhighlight */ }
        public virtual void OnSubmit() { /* click action */ }
    }
}