using CardFramework.Deck;
using CardFramework.RandomProvider;
using Game.Card;
using UnityEngine;

namespace Game
{
    public static class Services
    {
        public static SymbolBuilder SymbolBuilder;
        public static SlotMachineProvider SlotMachine;
        public static SlotMachineView MachineView;
        public static IRandomProvider Random;
        public static Deck<ICard> Deck;
        public static IPayTable PayTable;
        public static Camera MainCamera;
        
        public static ICardHand CurrentHand;
        public static ICardDatabaseService Cards;
        public static SoundService Sound;
    }
}