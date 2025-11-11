using System;
using Game.Core;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Symbol
    {
        
        public SymbolType Type;
        public SymbolVariantType Variant;
        public Guid Id;
        public Symbol()
        {
        }

        public Symbol(SymbolType type)
        {
            Id = Guid.NewGuid();
            Type = type;
        }
        public Symbol(SymbolType type,SymbolVariantType variant)
        {
            Id = Guid.NewGuid();
            Variant = variant;
            Type = type;
        }
    }
}