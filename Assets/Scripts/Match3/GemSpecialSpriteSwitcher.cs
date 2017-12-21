using System;
using UnityEngine;
using System.Linq;
using Match3Core;

namespace Match3Wrapper
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GemSpecialSpriteSwitcher : GemComponent
    {
        [Serializable]
        public class GemSpecialSprite
        {
            public GemSpecialType specialType;
            public Sprite sprite;
        }
        public GemSpecialSprite[] gemSpecialSprites;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public override void SetGemSpecialType(GemSpecialType specialType)
        {
            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer of " + gameObject.name + " is null");
            }
            GemSpecialSprite gemSpecialSprite = gemSpecialSprites.FirstOrDefault(g => g.specialType == specialType);
            if (gemSpecialSprite == null)
            {
                Debug.LogError("Failed to find sprite of type " + specialType);
                return;
            }
            spriteRenderer.sprite = gemSpecialSprite.sprite;
        }
    } 
}