using System;
using UnityEngine;

namespace Game.UI
{
    public class ShowDrawList:MonoBehaviour
    {
        [SerializeField]
        private CardListView View;

        private void OnMouseEnter()
        {
            var mousePos = Input.mousePosition;
            View.Show(mousePos,Services.Deck.Library.Items);
        }

        private void OnMouseExit()
        {
            View.Hide();
        }
    }
}