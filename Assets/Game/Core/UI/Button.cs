using Game.Pooling;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.Core.UI
{
    [RequireComponent(typeof(SelectableComponent))]
    public class Button:PoolableMonoBehaviour<Button>, ISelectable
    {
        [SerializeField] protected SelectableComponent Selectable;
        [SerializeField] protected UnityEvent OnClick;
        [Header("Visuals")]
        [SerializeField] protected GameObject Highlight;
        
        private void Awake()
        {
            if (Selectable == null)
                Selectable = GetComponent<SelectableComponent>();
            Selectable.Owner = this;
        }

        public SelectableNavigation Navigation => Selectable.Navigation;
        public void OnSelected()
        {
            if (Highlight != null)
                Highlight.SetActive(true);
            Selectable.OnSelected();
        }

        public void OnDeselected()
        {
            if (Highlight != null)
                Highlight.SetActive(false);
            Selectable.OnDeselected();
        }

        public void OnSubmit()
        {
            Debug.Log("Button submit");
            OnClick?.Invoke();
            SubmitAction();
        }

        protected virtual void SubmitAction()
        {
        }
    }
}