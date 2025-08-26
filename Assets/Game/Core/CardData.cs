using System.Collections.Generic;
using CardFramework;
using Game.Effect;
using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "Create CardData", fileName = "CardData", order = 0)]
    public class CardData:ScriptableObject
    {
        [SerializeField, ReadOnly]
        public string Id;
        public string Description;
        public List<CardEffect> Effects;
        
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Id))
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);
                Id = UnityEditor.AssetDatabase.AssetPathToGUID(path);
            }
        }
        #endif

        public void Setup()
        {
            for (var i = 0; i < Effects.Count; i++)
            {
                var cardEffect = Effects[i];
                Effects[i]=Instantiate(cardEffect);
            }
        }
    }
}