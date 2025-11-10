using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core;
using Game.Pooling;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SymbolView:PoolableMonoBehaviour<SymbolView>, ITransform
    {
        public const float Height = 1.28f;
        public SymbolType Type;
        [SerializeField] private SpriteRenderer Icon;
        [SerializeField] private TextMeshPro Id;
        [SerializeField] private TextMeshPro Count;
        
        [HideInInspector]
        public Symbol Model;

        public void Setup(Sprite sprite, int id,Symbol model)
        {
            Model = model;
            Icon.sprite = sprite;
            Id.SetText(id.ToString());
        }

        public void Setup(Symbol model)
        {
            Model = model;
            Icon.sprite = Services.SymbolBuilder.GetSymbolVisual(model.Type);
        }

        public void SetCount(int count)
        {
            Count.SetText(count.ToString());
            Count.gameObject.SetActive(true);
        }

        public void HideCount()
        {
            Count.gameObject.SetActive(false);
        }

        public void SetSortingLayer(int sortingLayer)
        {
            Icon.sortingLayerID = sortingLayer;
            Count.sortingLayerID = sortingLayer;
            Count.sortingOrder = 1;
        }

        public void Show(bool val)
        {
            Icon.enabled = val;
        }

        public void ModelChanged()
        {
            Icon.sprite=Services.SymbolBuilder.GetSymbolVisual(Model.Type);
        }

        public Vector3 Position
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public override void OnReturnToPool()
        {
            SetSortingLayer(SortingLayer.NameToID("Default"));
            HideCount();
            transform.localScale=Vector3.one;
        }
    }
}