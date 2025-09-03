using UnityEngine;

namespace Game.Core
{
    public interface ICardView
    {
        public void Bind(Card card);
        
        public void UpdateDurability();
        
        public void DiscardAnimation(Vector3 position);
        public void DrawAnimation(Vector3 position);

    }
}