using UnityEngine;

namespace Game.Core
{
    public interface ICardView
    {
        public void Bind(Card card);
        
        public void UpdateDurabilityView();
        
        public void DiscardAnimation();
        public void DrawAnimation();
        public void ReduceDurability();

    }
}