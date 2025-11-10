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
            Debug.Log("ShowDrawList submit action:"+Services.Deck.Library.Items.Count);
            View.Show(Vector3.zero, Services.Deck.Library.Items);
        }
    }
}