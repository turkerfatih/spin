using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SymbolBuilder:MonoBehaviour
    {
        [SerializeField] private List<SymbolVisual> SymbolVisuals;
        [SerializeField] private SymbolView SymbolViewPrefab;

        private Dictionary<SymbolType, SymbolVisual> dictionary;
        private void Awake()
        {
            Services.SymbolBuilder = this;
            dictionary = new Dictionary<SymbolType, SymbolVisual>();
            foreach (var symbolVisual in SymbolVisuals)
            {
                dictionary.Add(symbolVisual.Type,symbolVisual);
            }
        }

        public SymbolView GetView(Transform parent,Symbol symbol,int index)
        {
            var symbolType = symbol.Type;
            if (!dictionary.TryGetValue(symbolType, out var symbolVisual))
            {
                return null;
            }

            var symbolView = Instantiate(SymbolViewPrefab, parent);
            symbolView.Type = symbolType;
            symbolView.Setup(symbolVisual.Sprite,index,symbol);
            return symbolView;
        }
        

        public Sprite GetSymbolVisual(SymbolType type)
        {
 
             if (!dictionary.TryGetValue(type, out var symbolVisual))
             {
                 return null;
             }

             return symbolVisual.Sprite;
        }
    }
}