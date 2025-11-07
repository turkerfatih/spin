using System;
using UnityEngine;

namespace Game.Core.UI
{
    public class UIOverlay:MonoBehaviour
    {
        [SerializeField] private SpriteRenderer Renderer;

        public static UIOverlay Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Setup(Camera.main);
        }

        public void Setup(Camera cam)
        {
            FitSpriteToOrthographicCamera(cam,Renderer);
        }

        public static void FitSpriteToOrthographicCamera(Camera cam, SpriteRenderer spriteRenderer, float padding = 0f)
        {
            Sprite sprite = spriteRenderer.sprite;
            if (!sprite) return;

            float worldScreenHeight = cam.orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight * cam.aspect;

            Vector2 spriteSize = sprite.bounds.size;

            float scaleX = worldScreenWidth / spriteSize.x;
            float scaleY = worldScreenHeight / spriteSize.y;

            float scale = Mathf.Max(scaleX, scaleY) * (1f + padding);

            spriteRenderer.transform.localScale = new Vector3(scale, scale, 1f);

            // optional: center under camera
            Vector3 camPos = cam.transform.position;
            spriteRenderer.transform.position = new Vector3(camPos.x, camPos.y, spriteRenderer.transform.position.z);
        }
    }
    
}