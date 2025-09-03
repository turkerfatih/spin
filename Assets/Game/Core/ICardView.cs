using UnityEngine;

namespace Game.Core
{
    public interface ICardView
    {
        public void Bind(Card card);
        
        public void UpdateDurability();
        
        public void DiscardAnimation();
        public void DrawAnimation();

    }
}