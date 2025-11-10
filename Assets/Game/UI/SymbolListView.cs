using System.Collections.Generic;
using System.Linq;
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
        public  void Setup(List<Symbol> list)
        {
            
            if(list.Count <= 0)
                return;
            //Clear();
            var groupedSymbols = list
                .GroupBy(s => new { s.Type, s.Variant })
                .Select(g => new
                {
                    Type = g.Key.Type,
                    Variant = g.Key.Variant,
                    Count = g.Count(),
                })
                .ToList();
            
            var pool = Services.Pool;
            foreach (var groupItem in groupedSymbols)
            {
                var item=pool.Get(ItemPrefab);
                item.transform.SetParent(grid.Content);
                var symbol=new Symbol(groupItem.Type, groupItem.Variant);
                item.transform.localScale = new Vector3(2, 2, 2);
                item.Setup(symbol);
                item.SetSortingLayer(SortingLayerId);
                item.SetCount(groupItem.Count);
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