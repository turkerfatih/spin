using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Create Symbol Visual", fileName = "SymbolVisual", order = 0)]
    public class SymbolVisual:ScriptableObject
    {
        public SymbolType Type;
        public Sprite Sprite;
    }
}