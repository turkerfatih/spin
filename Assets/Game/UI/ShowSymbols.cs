using System;
using UnityEngine;

namespace Game.UI
{
    public class ShowSymbols: MonoBehaviour
    {
        [SerializeField]
        private SymbolsListView symbolsListView;

        private void OnMouseUp()
        {
            if (symbolsListView.gameObject.activeSelf)
            {
                symbolsListView.Hide();
            }else
                symbolsListView.Show(Services.Machine.GetSymbolsOnTheReel(0));
        }
    }
}