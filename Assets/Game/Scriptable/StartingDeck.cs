using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Create StartingDeck", fileName = "StartingDeck", order = 0)]
    public class StartingDeck:ScriptableObject
    {
        public List<StartingDeckItem> StartingDeckItems;
    }
}