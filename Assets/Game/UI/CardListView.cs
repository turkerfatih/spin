
using System;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.UI
{
    public class CardListView:MonoBehaviour
    {
        [SerializeField]
        private CardListItem itemPrefab;
        [SerializeField]
        private Transform itemContainer;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Show(Vector3 position, List<Card> cards)
        {
            if(cards.Count <= 0)
                return;
            transform.position=position;
            var pool = Services.Pool;
            foreach (var card in cards)
            {
                var item=pool.Get(itemPrefab);
                item.Setup(card);
                item.transform.SetParent(itemContainer, false);
            }
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            
            for (int i = itemContainer.childCount - 1; i >= 0; i--)
            {
                var child=itemContainer.GetChild(i);
                if (child.gameObject.TryGetComponent<CardListItem>(out var item))
                {
                    Debug.Log("remove item");
                    item.transform.SetParent(null);// unity UI parenting is problematic, so this is required
                    Services.Pool.Return(item);
                }
            }
            
            Debug.Log(itemContainer.childCount);
            gameObject.SetActive(false);
        }
    }
}