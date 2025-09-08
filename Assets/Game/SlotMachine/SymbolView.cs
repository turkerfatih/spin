using Game.Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SymbolView:PoolableMonoBehaviour
    {
        public const float Height = 1.28f;
        public SymbolType Type;
        [SerializeField] private SpriteRenderer Icon;
        [SerializeField] private TextMeshPro Id;

        public void Setup(Sprite sprite,int id)
        {
            Icon.sprite = sprite;
            Id.SetText(id.ToString());
        }

        public void Setup(Symbol symbol)
        {
            var sprite = Services.SymbolBuilder.GetSymbolVisual(symbol);
            Icon.sprite = sprite;
        }

        public void Show(bool val)
        {
            Icon.enabled = val;
        }
    }
}