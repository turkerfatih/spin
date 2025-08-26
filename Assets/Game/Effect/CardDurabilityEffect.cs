using CardFramework;
using Game.Event;
using UnityEngine;

namespace Game.Effect
{
    [CreateAssetMenu(menuName = "Create CardDurabilityEffect", fileName = "CardDurabilityEffect", order = 0)]
    public class CardDurabilityEffect:CardEffect
    {
        public int Durability;
        
        private int index = 0;
        public override void ApplyEffect(int slotIndex)
        {
            index = slotIndex;
            if(Durability>0)
                EventBus.OnReelSpinEnd += OnSpinEnd;
        }

        private void OnSpinEnd(int slotIndex)
        {
            if(index!=slotIndex)
                return;
            
            Durability--;
            Debug.Log("Durability:"+Durability);
            if (Durability <= 0)
            {
                /*var slot=Systems.Drop.GetSlot(slotIndex);
                var card=slot.GetCard();
                slot.Remove();
                EventBus.OnReelSpinEnd -= OnSpinEnd;*/
            }
        }
        
    }
}