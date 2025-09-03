using Game.Core;
using Game.Event;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class CoinDisplay:MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro Text;

        private double collected = 0;

        private void OnEnable()
        {
            EventBus.OnCoinGiven += OnCoinGiven;
            Text.SetText("");
        }
        private void OnDisable()
        {
            EventBus.OnCoinGiven -= OnCoinGiven;
        }

        private void OnCoinGiven(double givenAmount)
        {
            collected += givenAmount;
            Text.SetText(NumberFormatter.ToReadableString(collected));
        }
    }
}