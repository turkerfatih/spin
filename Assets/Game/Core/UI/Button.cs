using Game.Pooling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Core.UI
{
    [RequireComponent(typeof(SelectableComponent))]
    public class Button:PoolableMonoBehaviour, ISelectable
    {
        [SerializeField] private SelectableComponent Selectable;

        public SelectableNavigation Navigation => Selectable.Navigation;
        public void OnSelected() => Selectable.OnSelected();
        public void OnDeselected() => Selectable.OnDeselected();
        public void OnSubmit() => Selectable.OnSubmit();
    }
}