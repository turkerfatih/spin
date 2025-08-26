using System;
using TMPro;
using UnityEngine;

namespace Game.Core
{
    public class Card:MonoBehaviour
    {
        public TextMeshPro label;
        public TextMeshPro info;
        [NonSerialized]
        public CardData Data;
        public void Load(CardData data)
        {
            Data = Instantiate(data);
            Data.Setup();
            label.SetText(data.Description);
        }
    }
}