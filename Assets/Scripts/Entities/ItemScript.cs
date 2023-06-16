using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemScript : EntityScript
{
    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
