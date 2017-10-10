using System;
using UnityEngine;

public class GemSpriteSwitcher : GemComponent
{
    public Sprite[] gemSprites;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public override void SetGemType(int type)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer of " + gameObject.name + " is null");
        }
        if (gemSprites.Length <= type)
        {
            Debug.LogError("Invalid value of type, type value is more than gemSprites length");
        }
        spriteRenderer.sprite = gemSprites[type];
    }
}