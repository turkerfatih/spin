using Game.Core;
using Game.Event;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class RoundScoreDisplay:MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro Text;

        private double collected = 0;

        private void OnEnable()
        {
            EventBus.OnScoreGiven += OnScoreGiven;
            Text.SetText("0");
        }
        private void OnDisable()
        {
            EventBus.OnScoreGiven -= OnScoreGiven;
        }

        private void OnScoreGiven(double givenAmount)
        {
            collected += givenAmount;
            Text.SetText(NumberFormatter.ToReadableString(collected));
        }
    }
}