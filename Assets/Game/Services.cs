using CardFramework.Deck;
using CardFramework.RandomProvider;
using Game.Actions;
using Game.Core;
using UnityEngine;

namespace Game
{
    public static class Services
    {
        public static SymbolBuilder SymbolBuilder;
        public static SlotMachine Machine;
        public static SlotMachineView MachineView;
        public static IRandomProvider Random;
        public static Deck<Card> Deck;
        public static PayTable PayTable;
        public static Camera MainCamera;
        
        public static ICardHand CurrentHand;
        public static ICardDatabaseService Cards;
        public static SoundService Sound;
        public static ActionQueue Actions;
    }
}