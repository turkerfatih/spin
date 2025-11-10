
using System;
using System.Collections.Generic;
using Game.Core;
using Game.Core.UI;
using Game.UI.Component;
using UnityEngine;

namespace Game.UI
{
    public class CardListView:PopupWindow
    {
        [SerializeField]
        private CardView ItemPrefab;
        private GridView grid;
        
        private List<CardView> items = new List<CardView>(30);
        

        private void Awake()
        {
            grid = GetComponent<GridView>();
            gameObject.SetActive(false);
        }

        

        public void Show(Vector3 position, List<Card> cards)
        {
            if(cards.Count <= 0)
                return;
            //Clear();
            transform.position=position;
            var pool = Services.Pool;
            foreach (var card in cards)
            {
                var item=pool.Get(ItemPrefab);
                item.Bind(card);
                item.SetSortingLayer(LayerHelper.Popup);
                item.SetOrder(1);
                item.transform.SetParent(grid.Content);
                items.Add(item);
            }
            grid.Setup(items);
            base.Show();
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
            Debug.Log("CardListView clear");
            for (var index = items.Count - 1; index >= 0; index--)
            {
                var item = items[index];
                Services.Pool.Return(item);
            }
            items.Clear();
        }
    }
}