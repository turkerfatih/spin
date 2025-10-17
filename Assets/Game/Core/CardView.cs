using System;
using DG.Tweening;
using Game.Pooling;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Core
{
    public class CardView:PoolableMonoBehaviour,ICardView,ITransform
    {
        public Card Model { get; private set; }
        
        private const float maxYRotation = 20f; // how much tilt (degrees)
        private  const float smoothSpeed = 10f;  
        public static float Width = 1f;
        public TextMeshPro label;
        public TextMeshPro durability;
        public SpriteRenderer Visual;
        
        [SerializeField] private Transform rotater;
        
        [SerializeField]private SortingGroup sortingGroup;
        
        [SerializeField] private SpriteRenderer Background;
        [SerializeField] private SpriteRenderer DurabilityBackground;
        [SerializeField] private SpriteRenderer Shadow;
        [SerializeField] private Material MaskedLabelFont;
        [SerializeField] private Material MaskedDurabilityFont;
        
        private Material labelFont;
        private Material durabilityFont;
        private bool masked=false;
        public bool IsMasked=>masked;
        
        private void Awake()
        {
            labelFont = label.fontSharedMaterial;
            durabilityFont = durability.fontSharedMaterial;
        }
        

        public void Bind(Card card)
        {
            Model = card;
            label.text = card.Definition.CardName;
            UpdateDurability();
            Visual.sprite = card.Definition.Icon;
        }

        public void UnBind()
        {
            Model = null;
            Destroy(gameObject);
        }

        public void UpdateDurability()
        {
            if(Model.Durability<=0)
                return;
            durability.SetText(Model.Definition.Durability.ToString());
        }

        public void DiscardAnimation()
        {
            gameObject.SetActive(false);
        }

        public void DrawAnimation()
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);
        }
        

        public void SetOrder(int index)
        {
            sortingGroup.sortingOrder = index;
        }

        public void SetSortingLayer(int sortingLayer)
        {
            sortingGroup.sortingLayerID = sortingLayer;
        }

        public void UpdateRotate(Vector3 localPosRef)
        {

            // Map mouse X position (relative to card) to [-1, 1] range
            float halfWidth = rotater.localScale.x * 0.5f; // assumes pivot center
            float normalizedX = Mathf.Clamp(localPosRef.x / halfWidth, -1f, 1f);

            // Target rotation
            Quaternion targetRot =  Quaternion.Euler(0f, normalizedX * maxYRotation, 0f);

            // Smooth transition
            rotater.localRotation = Quaternion.Lerp(rotater.localRotation, targetRot, Time.deltaTime * smoothSpeed);
        }

        public void SetMasked(bool isMasked)
        {
            masked=isMasked;
            var maskInteraction=masked?SpriteMaskInteraction.VisibleInsideMask:SpriteMaskInteraction.None;
            Background.maskInteraction = maskInteraction;
            DurabilityBackground.maskInteraction = maskInteraction;
            Visual.maskInteraction = maskInteraction;
            Shadow.maskInteraction = maskInteraction;
            label.fontSharedMaterial = masked ? MaskedLabelFont : labelFont;
            durability.fontSharedMaterial = masked ? MaskedDurabilityFont : durabilityFont;
        }

        public Vector3 Position
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }
    }
}