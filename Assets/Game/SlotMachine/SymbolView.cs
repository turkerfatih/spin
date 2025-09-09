using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core;
using Game.Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SymbolView:PoolableMonoBehaviour
    {
        public const float Height = 1.28f;
        public SymbolType Type;
        [SerializeField] private SpriteRenderer Icon;
        [SerializeField] private TextMeshPro Id;

        public void Setup(Sprite sprite,int id)
        {
            Icon.sprite = sprite;
            Id.SetText(id.ToString());
        }
        
        public void Show(bool val)
        {
            Icon.enabled = val;
        }
        
    }
}