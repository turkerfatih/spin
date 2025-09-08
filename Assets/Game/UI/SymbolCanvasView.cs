using Game.Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SymbolCanvasView:PoolableMonoBehaviour
    {
        [SerializeField] private Image Icon;
        [SerializeField] private TextMeshProUGUI Id;
        
        public void Setup(Symbol symbol)
        {
            var sprite = Services.SymbolBuilder.GetSymbolVisual(symbol);
            Icon.sprite = sprite;
        }
    }
}