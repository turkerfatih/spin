using System;
using Game.Core.UI;
using UnityEngine;

namespace Game.UI
{
    public class ShowDrawList:Button
    {
        [SerializeField]
        private CardListView View;

        protected override void SubmitAction()
        {
            Debug.Log("ShowDrawList submit action");
            View.Show(transform.position,Services.Deck.Library.Items);
        }
    }
}