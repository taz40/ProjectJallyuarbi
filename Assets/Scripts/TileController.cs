using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * The format for the map file is as follows:
 * The file consists of a list of comma seperated values.
 * The first 2 values represent the width and height of the map.
 * The subsequent values represent the tile type starting with the bottom left corner, and working to the right and looping to the next line after reaching the end of the map.
 * 
 */

public class TileController : MonoBehaviour {

    int width, height;  //The width and height of the tile map

    //A list of colors that represent each tile, the index in the array is the tile index (Will be replaced with textures later).
    //Color[] colors = { Color.white, Color.black, Color.red, Color.blue };

    //A list that determines if any given tile is collidable. The index in the array is the tile index.
    bool[] collidable = { false, false, true, false };

    public GameObject tilePrefab; //A referance to the prefab used to represent tiles.
    public TextAsset mapFile; //A reference to the map file to be loaded.
    Sprite[] sprite;
    List<GameObject> objects = new List<GameObject>();

    void Start() {
        sprite = Resources.LoadAll<Sprite>("Sprites/Tiles/TileSet");
        GenerateTileMap(); //Generate the tile map as soon as the game starts.
    }

    public void DeleteMapEditor(){
        while(transform.childCount != 0){
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        foreach(GameObject go in objects){
            DestroyImmediate(go);
        }
    }

    public void GenMapEdit(){
        sprite = Resources.LoadAll<Sprite>("Sprites/Tiles/TileSet");
        //First we iterate over all our children and delete them. This is done in case the map has already been previously generated.

        while(transform.childCount != 0){
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        foreach(GameObject go in objects){
            Destroy(go);
        }

        string data = mapFile.text; //We load the text out of the file.

        string[] tiles = data.Split(','); //Here we split it into smaller strings by cutting the string every time there is a ','

        width = int.Parse(tiles[0]); //Here we load the width of the map from the file
        height = int.Parse(tiles[1]); //Here we load the height of the map from the file


        //Now iterate over every tile and create it.
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                GameObject go = Instantiate(tilePrefab, new Vector3(x, y, 1), Quaternion.identity, transform); //Here we create a new object from the prefab at the correct x,y position, with us as a parrent.
                go.name = "Tile_" + x + "_" + y; //We set the tile's name to something understandable to simplify debugging.

                SpriteRenderer sr = go.GetComponent<SpriteRenderer>(); //We get the sprite renderer off of the new game object.
                //sr.sprite = sprite;
                int dataIndex = 2 + ((y * width) + x); //This determines the index in the tiles array that contains our tile's data
                int collidable = dataIndex + (width*height);
                int rotationIndex = collidable + (width*height);
                int tileType = int.Parse(tiles[dataIndex]); //This is the type of tile this is. It corresponds to the colors and collidable arrays above.
                //sr.color = colors[tileType]; //Here we set the tile's color to the correct color based on the array.
                sr.sprite = sprite[tileType];
                go.transform.Rotate(0, 0, 90*int.Parse(tiles[rotationIndex]));

                //Here we check if the tile should be collidable. If it should then we enable its BoxCollider2D
                if (tiles[collidable] == "True") {
                    go.GetComponent<BoxCollider2D>().enabled = true;
                }

            }
        }

        for(int i = 2+(height*width)*3; i < tiles.Length; i++){
            data = tiles[i];
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/"+data.Split('/')[0]));
            go.GetComponent<PlaceableObject>().LoadFromString(data);
            objects.Add(go);
        }
    }

    
    void Update() {

    }

    //A function that generates the tile map from the map file.
    void GenerateTileMap() {
        //First we iterate over all our children and delete them. This is done in case the map has already been previously generated.
        for(int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
        foreach(GameObject go in objects){
            Destroy(go);
        }

        string data = mapFile.text; //We load the text out of the file.

        string[] tiles = data.Split(','); //Here we split it into smaller strings by cutting the string every time there is a ','

        width = int.Parse(tiles[0]); //Here we load the width of the map from the file
        height = int.Parse(tiles[1]); //Here we load the height of the map from the file


        //Now iterate over every tile and create it.
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                GameObject go = Instantiate(tilePrefab, new Vector3(x, y, 1), Quaternion.identity, transform); //Here we create a new object from the prefab at the correct x,y position, with us as a parrent.
                go.name = "Tile_" + x + "_" + y; //We set the tile's name to something understandable to simplify debugging.

                SpriteRenderer sr = go.GetComponent<SpriteRenderer>(); //We get the sprite renderer off of the new game object.
                //sr.sprite = sprite;
                int dataIndex = 2 + ((y * width) + x); //This determines the index in the tiles array that contains our tile's data
                int collidable = dataIndex + (width*height);
                int rotationIndex = collidable + (width*height);
                int tileType = int.Parse(tiles[dataIndex]); //This is the type of tile this is. It corresponds to the colors and collidable arrays above.
                //sr.color = colors[tileType]; //Here we set the tile's color to the correct color based on the array.
                sr.sprite = sprite[tileType];
                go.transform.Rotate(0, 0, 90*int.Parse(tiles[rotationIndex]));

                //Here we check if the tile should be collidable. If it should then we enable its BoxCollider2D
                if (tiles[collidable] == "True") {
                    go.GetComponent<BoxCollider2D>().enabled = true;
                }

            }
        }
        for(int i = 2+(height*width)*3; i < tiles.Length; i++){
            data = tiles[i];
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/"+data.Split('/')[0]));
            go.GetComponent<PlaceableObject>().LoadFromString(data);
            objects.Add(go);
        }

    }

}
