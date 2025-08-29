using System;
using UnityEngine;

namespace Game
{
    public class Symbol
    {
        
        public SymbolType Type;
        public Guid Id;
        public Symbol()
        {
        }

        public Symbol(SymbolType type)
        {
            Id = Guid.NewGuid();
            Type = type;
        }
    }
}