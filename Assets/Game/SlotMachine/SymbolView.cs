using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SymbolView:MonoBehaviour
    {
        public const float Height = 2.5f;
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