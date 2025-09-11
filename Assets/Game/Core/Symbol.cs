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

        public Symbol Clone()
        {
            return (Symbol)MemberwiseClone();
        }

        public void ChangeType(SymbolType type)
        {
            this.Type = type;
        }
        
        public Action OnChange;
        
    }
}