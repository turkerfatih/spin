using System;
using UnityEngine;

namespace Game.UI
{
    public class ShowDiscardList:MonoBehaviour
    {
        [SerializeField]
        private CardListView View;

        private void OnMouseEnter()
        {
            var mousePos = Input.mousePosition;
            View.Show(mousePos,Services.Deck.Discarded.Items);
        }

        private void OnMouseExit()
        {
            View.Hide();
        }
    }
}