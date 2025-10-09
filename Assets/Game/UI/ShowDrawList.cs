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
           
        }

        private void OnMouseExit()
        {
            View.Hide();
        }


        private void OnMouseUp()
        {
            View.Show(transform.position,Services.Deck.Library.Items);
        }
    }
}