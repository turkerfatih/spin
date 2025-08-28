using System;
using TMPro;
using UnityEngine;

namespace Game.Core
{
    public class Card:MonoBehaviour
    {
        public static float Width = 1f;
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

        public void ResetCard()
        {
           var data= Services.Cards.GetCardData(Data.Id); 
           Load(data);
        }

        public void ChangeId(int id)
        {
            info.SetText(id.ToString());
        }
    }
}