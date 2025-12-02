using UnityEngine;

namespace Game.Core
{
    public static class SortLayerHelper
    {
        public static int Popup;
        public static int Card;
        public static void Setup()
        {
            Popup=SortingLayer.NameToID("Popup");
            Card=SortingLayer.NameToID("Card");
        }

    }
}