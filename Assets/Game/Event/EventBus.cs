using System;
using Game.Core;
using UnityEngine;

namespace Game.Event
{
    public  class EventBus
    {
        public static Action<int> OnReelSpinEnd;
        public static Action OnNewGameLoaded;
        public static Action OnSlotMachineLoaded;
        public static Action<int> OnDrawPileChanged;
        public static Action<int> OnDiscardPileChanged;
        public static Action<double> OnScoreGiven;
        public static Action<RoundData> RoundDataChange;
        public static Action<int> OnCreditsChanged;
        
        public static Action<CardView> OnDragCancel;
        public static Action<int> OnDropSlotSelected;
        public static Action<CardView,int> OnCardDroppedToSlot;
        public static Action<float> OnLightValueChanged;
        public static Action<Vector3> OnLightRotationChanged;
    }
}