using System.Collections.Generic;
using Game.Core;
using Game.Core.UI;
using Game.UI.Component;
using UnityEngine;

namespace Game.UI
{
    public class SymbolListView:PopupWindow
    {
        [SerializeField]
        private SymbolView ItemPrefab;
        private GridView grid;
        private List<SymbolView> items = new List<SymbolView>(30);


        protected override void Awake()
        {
            base.Awake();
            grid = GetComponent<GridView>();
            gameObject.SetActive(false);
        }
        public  void Setup(List<Symbol> symbols)
        {
            if(symbols.Count <= 0)
                return;
            //Clear();

            var pool = Services.Pool;
            foreach (var symbol in symbols)
            {
                var item=pool.Get(ItemPrefab);
                item.transform.SetParent(grid.Content);
                item.Setup(symbol);
                item.SetSortingLayer(SortingLayerId);
                items.Add(item);
            }
            grid.Setup(items);
            Show();
        }
        private void OnDisable()
        {
            Hide();
            Clear();
        }

        private void Clear()
        {

            if(items.Count==0)
                return;
            Debug.Log("SymbolsListView clear");
            for (var index = items.Count - 1; index >= 0; index--)
            {
                var item = items[index];
                Services.Pool.Return(item);
            }
            items.Clear();
        }
    }
}