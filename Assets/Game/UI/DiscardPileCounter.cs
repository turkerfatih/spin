using System;
using Game.Event;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class DiscardPileCounter:MonoBehaviour
    {
        [SerializeField] private TextMeshPro Text;

        private void OnEnable()
        {
            EventBus.OnDiscardPileChanged+=UpdateText;
        }

        private void OnDisable()
        {  
            EventBus.OnDiscardPileChanged-=UpdateText;
        }

        private void UpdateText(int value)
        {
            Text.SetText(value.ToString());
        }
    }
}