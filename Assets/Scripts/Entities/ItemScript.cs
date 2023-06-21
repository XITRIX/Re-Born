using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemScript : EntityScript
{
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        GlobalDirector.Shared.GameObjectsStash[entityId] = gameObject;
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
