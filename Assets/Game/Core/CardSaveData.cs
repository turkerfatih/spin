using System;
using System.Collections.Generic;

namespace Game.Core
{
    [Serializable]
    public class CardSaveData
    {
        public string DefinitionId;
        public Guid InstanceId;
        public int Durability;
        public List<CardAttributeSaveData> Attributes = new();
    }
}