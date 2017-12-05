using System;
using UnityEngine;
using System.Linq;
using Match3Core;

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

    public override void SetGemSpecialType(GemSpecialType specialtype)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer of " + gameObject.name + " is null");
        }
        GemSpecialSprite gemSpecialSprite = gemSpecialSprites.FirstOrDefault(g => g.specialType == specialtype);
        if (gemSpecialSprite == null)
        {
            Debug.LogError("Failed to find sprite of type " + specialtype);
            return;
        }
        spriteRenderer.sprite = gemSpecialSprite.sprite;
    }
}