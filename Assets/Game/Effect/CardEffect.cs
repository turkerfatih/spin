using UnityEngine;

namespace Game.Effect
{
    public abstract class CardEffect:ScriptableObject,ICardEffect
    {
        public abstract void ApplyEffect(int slotIndex);
        

    }
}