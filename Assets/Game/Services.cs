using System.Collections.Generic;
using CardFramework.Deck;
using CardFramework.RandomProvider;
using Game.Actions;
using Game.Core;
using Game.Pooling;
using UnityEngine;

namespace Game
{
    public static class Services
    {
        public static GameSetup GameSetup;
        public static SymbolBuilder SymbolBuilder;
        public static SlotMachine Machine;
        public static SlotMachineView MachineView;
        public static IRandomProvider Random;
        public static List<Card> Cards;
        public static Deck<Card> PlayDeck;
        public static HandManager Hand;
        public static PayTable PayTable;
        public static Camera MainCamera;
        public static IPoolManager Pool;
        
        public static ICardHand CurrentHand;
        public static ICardDatabaseService CardsDatabase;
        public static SoundService Sound;
        public static ActionQueue Actions;
        public static RoundData Round;
        public static int HandSize;

    }
}