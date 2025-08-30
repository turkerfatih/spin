using UnityEngine;

namespace Game.Effects
{
    public abstract class CardEffectDefinition : ScriptableObject
    {
        public abstract IGameEffect CreateRuntimeEffect();
    }
}