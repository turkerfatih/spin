using System;

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
    }
}