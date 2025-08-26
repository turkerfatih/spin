using System;
using UnityEngine;

namespace Game
{
    public class Symbol
    {
        public Guid Id;
        public SymbolType Type;

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