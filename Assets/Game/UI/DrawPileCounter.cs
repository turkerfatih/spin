using System;
using Game.Event;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class DrawPileCounter:MonoBehaviour
    {
        [SerializeField] private TextMeshPro Text;

        private void OnEnable()
        {
            EventBus.OnDrawPileChanged+=UpdateText;
        }

        private void OnDisable()
        {  
            EventBus.OnDrawPileChanged-=UpdateText;
        }

        private void UpdateText(int value)
        {
            Text.SetText(value.ToString());
        }
    }
}