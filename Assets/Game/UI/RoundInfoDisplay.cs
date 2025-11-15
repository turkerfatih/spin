using Game.Event;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI
{
    public class RoundInfoDisplay:MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro RoundNumberLabel;
        [SerializeField]
        private TextMeshPro RoundNumberText;
        [SerializeField]
        private TextMeshPro RoundTargetLabel;
        [SerializeField]
        private TextMeshPro RoundTargetText;
        private int roundNumber = 0;
        
        private void OnEnable()
        {
            EventBus.RoundDataChange += OnRoundDataChange;
            RoundNumberText.SetText("");
        }
        private void OnDisable()
        {
            EventBus.RoundDataChange -= OnRoundDataChange;
        }
        private void OnRoundDataChange(RoundData data)
        {
            if(roundNumber==data.Number)
                return;
            roundNumber = data.Number;
            RoundNumberText.SetText(roundNumber.ToString());
            RoundTargetText.SetText(data.Target.ToString());
        }
    }
}