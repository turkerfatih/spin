using Game.Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SymbolCanvasView:PoolableMonoBehaviour<SymbolCanvasView>
    {
        [SerializeField] private Image Icon;
        [SerializeField] private TextMeshProUGUI Id;
        
        public void Setup(SymbolType symbol,int count)
        {
            var sprite = Services.SymbolBuilder.GetSymbolVisual(symbol);
            Id.SetText($"x{count}");
            Icon.sprite = sprite;
        }
    }
}