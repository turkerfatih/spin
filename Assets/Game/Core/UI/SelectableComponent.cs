using UnityEngine;

namespace Game.Core.UI
{
    public class SelectableComponent : MonoBehaviour, ISelectable
    {
        [SerializeField] private SelectableNavigation navigation = new();
        public ISelectable Owner { get; set; }
        public SelectableNavigation Navigation => navigation;

        public virtual void OnSelected() { /* highlight visuals */ }
        public virtual void OnDeselected() { /* unhighlight */ }

        public void OnSubmit()
        {

            if (Owner != null)
            {
                Owner.OnSubmit();
                return;
            }
            HandleSubmit();
        }

        protected virtual void HandleSubmit()
        {
            
        }
    }
}