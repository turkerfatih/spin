using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Card Database")]
    public class CardDatabase : ScriptableObject
    {
        public List<CardDefinition> AllCards;
    }
}