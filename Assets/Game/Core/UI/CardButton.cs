using System;
using UnityEngine;

namespace Game.Core.UI
{
    public class CardButton:Button
    {
        public ICardSelectionHandler SelectionHandler { get; set; }
        public int Index { get; set; }
        public CardView View { get; set; }
        protected override void SubmitAction()
        {
            Debug.Log("Card Button submitted");
            SelectionHandler.OnCardSelected(this);
        }
    }
}