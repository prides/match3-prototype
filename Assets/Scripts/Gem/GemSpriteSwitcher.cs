using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GemSpriteSwitcher : GemComponent
{
    [Serializable]
    public class GemSprite
    {
        public GemType type;
        public Sprite sprite;
    }
    public GemSprite[] gemSprites;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public override void SetGemType(GemType type)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer of " + gameObject.name + " is null");
        }
        GemSprite gemSprite = gemSprites.FirstOrDefault(g => g.type == type);
        if (gemSprite == null)
        {
            Debug.LogError("Failed to find sprite of type " + type);
            return;
        }
        spriteRenderer.sprite = gemSprite.sprite;
    }
}