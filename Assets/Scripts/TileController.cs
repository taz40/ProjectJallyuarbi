using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

    int width, height;

    Color[] colors = { Color.white, Color.black, Color.red, Color.blue };

    public GameObject tilePrefab;
    public TextAsset mapFile;

    void Start() {
        GenerateTileMap();
    }

    
    void Update() {

    }

    void GenerateTileMap() {
        for(int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }

        string data = mapFile.text;

        string[] tiles = data.Split(',');

        width = int.Parse(tiles[0]);
        height = int.Parse(tiles[1]);

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                GameObject go = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                go.name = "Tile_" + x + "_" + y;

                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                
                sr.color = colors[int.Parse(tiles[2+((y * width) + x)])];

            }
        }

    }

}
