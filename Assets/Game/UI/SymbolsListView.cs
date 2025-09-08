using System;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;
using System.Linq;
namespace Game.UI
{
    public class SymbolsListView:MonoBehaviour
    {
        [SerializeField]
        private SymbolCanvasView itemPrefab;
        [SerializeField]
        private Transform itemContainer;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Show( List<Symbol> symbols)
        {
            if(symbols.Count <= 0)
                return;
            var pool = Services.Pool;

            foreach (var symbol in symbols)
            {
                var item=pool.Get(itemPrefab);
                item.Setup(symbol);
                item.transform.SetParent(itemContainer, false);
            }
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            
            for (int i = itemContainer.childCount - 1; i >= 0; i--)
            {
                var child=itemContainer.GetChild(i);
                if (child.gameObject.TryGetComponent<SymbolCanvasView>(out var item))
                {
                    item.transform.SetParent(null);// unity UI parenting is problematic, so this is required
                    Services.Pool.Return(item);
                }
            }
            
            gameObject.SetActive(false);
        }
    }
}