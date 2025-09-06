
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

        public void Show(Vector3 position, List<Card> cards)
        {
            foreach (var card in cards)
            {
                
            }
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}