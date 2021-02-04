using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

    int width = 10;
    int height = 10;

    public GameObject tilePrefab;

    void Start() {
        GenerateTileMap();
    }

    
    void Update() {

    }

    void GenerateTileMap() {
        for(int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                GameObject go = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                go.name = "Tile_" + x + "_" + y;
            }
        }

    }

}
