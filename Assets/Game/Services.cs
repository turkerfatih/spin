using CardFramework.Deck;
using CardFramework.RandomProvider;
using Game.Core;
using UnityEngine;

namespace Game
{
    public static class Services
    {
        public static SymbolBuilder SymbolBuilder;
        public static SlotMachineProvider SlotMachine;
        public static SlotMachineView MachineView;
        public static IRandomProvider Random;
        public static Deck<Card> Deck;
        public static IPayTable PayTable;
        public static Camera MainCamera;
        
        public static ICardHand CurrentHand;
        public static ICardDatabaseService Cards;
        public static SoundService Sound;
    }
}