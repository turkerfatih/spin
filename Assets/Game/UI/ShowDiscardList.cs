using System;
using UnityEngine;
using Game.Core.UI;
namespace Game.UI
{
    public class ShowDiscardList:Button
    {
        [SerializeField]
        private CardListView View;

        protected override void SubmitAction()
        {
            Debug.Log("ShowDrawList submit action:"+Services.Deck.Discarded.Items.Count);
            View.Show(Vector3.zero, Services.Deck.Discarded.Items);
        }
    }
}