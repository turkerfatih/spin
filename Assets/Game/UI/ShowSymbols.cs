using Game.Core.UI;
using UnityEngine;

namespace Game.UI
{
    public class ShowSymbols:Button
    {
        [SerializeField]
        private SymbolListView View;

        protected override void SubmitAction()
        {
            View.Setup(Services.Machine.GetSymbolsOnTheReel(0));
        }
    }
}