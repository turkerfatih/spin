using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Component
{
    public class GridView : MonoBehaviour
    {
        [Header("Grid Settings")] 
        public float ItemWidth = 1f;
        public float ItemHeight = 1f;
        public float HorizontalSpacing = 0.1f;
        public float VerticalSpacing = 0.1f;

        [Header("References")] 
        public SpriteRenderer Background;
        public Transform Content; // Holds items; we move this when scrolling
        public Transform ScrollThumb;// We need to move this locally according to scroll, hide it if not necessary
        
        private float scrollTopLimit;
        private float scrollBottomLimit;
        private Vector2 bgSize;
        private float thumbHalfHeight=0.5f;
        private float thumbHalfWidth = 1f;

        public void Setup(List<Transform> items)
        {
            if (items == null || items.Count == 0) return;
            if (Background == null)
            {
                Debug.LogWarning("GridView.Setup: Background not assigned!");
                return;
            }
            // Get usable background area in world/local units
            bgSize = Background.size;
            bgSize.y -= thumbHalfWidth ;
            var thumbPos=ScrollThumb.position;
            thumbPos.x = bgSize.x/2f+thumbHalfWidth;
            ScrollThumb.localPosition = thumbPos;
            // Compute how many columns/rows fit inside background
            int columns = Mathf.Max(1, Mathf.FloorToInt((bgSize.x + HorizontalSpacing) / (ItemWidth + HorizontalSpacing)));
            int rows = Mathf.Max(1, Mathf.FloorToInt((bgSize.y + VerticalSpacing) / (ItemHeight + VerticalSpacing)));

            // For scrollable content, we can exceed the visible row count
            // but initial positioning is still continuous grid
            for (int i = 0; i < items.Count; i++)
            {
                int col = i % columns;
                int row = i / columns;

                float x = col * (ItemWidth + HorizontalSpacing);
                scrollBottomLimit = -row * (ItemHeight + VerticalSpacing);

                items[i].localPosition = new Vector3(x, scrollBottomLimit, 0f);
            }

            // Center content relative to background bounds
            CenterGrid(columns, rows);
        }

        private void CenterGrid(int columns, int rows)
        {
            Vector2 bgSize = Background.size;

            float gridWidth = columns * ItemWidth + (columns - 1) * HorizontalSpacing;
            float gridHeight = rows * ItemHeight + (rows - 1) * VerticalSpacing;

            // Compute offset so grid starts top-left inside background
            // or center it if smaller than background
            float offsetX = -bgSize.x / 2f + ItemWidth / 2f;
            float offsetY = bgSize.y / 2f - ItemHeight / 2f;

            // If the grid is smaller than the background, we center it
            if (gridWidth < bgSize.x)
                offsetX += (bgSize.x - gridWidth) / 2f;
            if (gridHeight < bgSize.y)
                offsetY -= (bgSize.y - gridHeight) / 2f;

            var pos = new Vector3(offsetX, offsetY, 0f);
            Content.localPosition = pos;
            scrollTopLimit = offsetY;
            Debug.Log("rows:"+rows+"Grid Height:"+gridHeight);
            scrollBottomLimit=  Mathf.Abs( scrollBottomLimit)-offsetY;
            Debug.Log("Top:"+scrollTopLimit+" Bottom:"+scrollBottomLimit);
            ScrollThumb.gameObject.SetActive(Scrollable);
        }

        public void Scroll(float f)
        {
            if(!ScrollThumb.gameObject.activeSelf)
                return;
            var p = Content.localPosition;
            p.y=Mathf.Clamp(p.y+f,scrollTopLimit,scrollBottomLimit);
            Content.localPosition = p;
            var stCurPos=ScrollThumb.transform.localPosition;
            var nsp = GetNormalizedScrollPosition();
            var halfHeight = bgSize.y / 2f;
            var stPosY=Mathf.Lerp(-halfHeight+thumbHalfHeight, halfHeight-thumbHalfHeight, nsp);
            ScrollThumb.transform.localPosition=new Vector3(stCurPos.x,stPosY,stCurPos.z);

        }
        public float GetNormalizedScrollPosition()
        {
            if (!ScrollThumb.gameObject.activeSelf)
                return 0f;
            return Mathf.InverseLerp(scrollTopLimit,scrollBottomLimit,Content.localPosition.y);
        }

        private bool Scrollable => !Mathf.Approximately(scrollTopLimit, scrollBottomLimit);
    }
}
