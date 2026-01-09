using System.Collections.Generic;
using CardFramework;
using Game.Effect;
using Game.Effects;
using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "Game/Card Definition")]
    public class CardDefinition : ScriptableObject
    {
        [SerializeField, ReadOnly]
        public string Id;
        public string CardName;
        public Sprite Icon;
        public int Durability = 1;
        public int Charge = -1;
        
        [TextArea] public string Description;
        
        [SerializeReference] public List<CardEffect> Effects;
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Id))
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);
                Id = UnityEditor.AssetDatabase.AssetPathToGUID(path);
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
        #endif
        

    }
}