using System;
using Game.Core;

namespace Game.Event
{
    public  class EventBus
    {
        public static Action<int> OnReelSpinEnd;
        public static Action OnNewGameLoaded;
        public static Action OnSlotMachineLoaded;
        public static Action<int> OnDrawPileChanged;
        public static Action<int> OnDiscardPileChanged;
        public static Action<double> OnCoinGiven;
        
        public static Action<Card> OnCardAddToHand;
        public static Action<Card> OnDragCancel;
        public static Action<int> OnDropSlotSelected;
    }
}