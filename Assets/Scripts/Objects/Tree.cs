using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : PlaceableObject {

    public string assetName;

    void Start() {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Objects/"+assetName);
        GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(0,sprites.Length)];
    }
}
