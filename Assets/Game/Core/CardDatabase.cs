using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "Create CardDatabase", fileName = "CardDatabase", order = 0)]
    public class CardDatabase:ScriptableObject
    {
        public List<CardData> Cards;
    }
}