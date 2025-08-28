using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Core
{
    public class Card:MonoBehaviour
    {
        private const float maxYRotation = 20f; // how much tilt (degrees)
        private  const float smoothSpeed = 10f;  
        public static float Width = 1f;
        public TextMeshPro label;
        public TextMeshPro info;
        public SpriteRenderer Visual;
        [NonSerialized]
        public CardData Data;

        [SerializeField] private Transform rotater;

        private SortingGroup sortingGroup;
        private void Awake()
        {
            sortingGroup = GetComponent<SortingGroup>();
        }

        public void Load(CardData data)
        {
            Data = Instantiate(data);
            Data.Setup();
            label.SetText(data.Description);
            Visual.sprite = data.Sprite;
        }

        public void ResetCard()
        {
           var data= Services.Cards.GetCardData(Data.Id); 
           Load(data);
        }

        public void ChangeId(int id)
        {
            info.SetText(id.ToString());
        }

        public void SetOrder(int index)
        {
            sortingGroup.sortingOrder = index;
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

    }
}