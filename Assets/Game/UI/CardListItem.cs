using Game.Core;
using Game.Pooling;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class CardListItem:PoolableMonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        public void Setup(Card card)
        {
            text.SetText(card.Definition.CardName);
        }
    }
}