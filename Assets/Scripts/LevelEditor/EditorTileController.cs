using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

/*
 * 
 * The format for the map file is as follows:
 * The file consists of a list of comma seperated values.
 * The first 2 values represent the width and height of the map.
 * The subsequent values represent the tile type starting with the bottom left corner, and working to the right and looping to the next line after reaching the end of the map.
 * 
 */

public class EditorTileController : MonoBehaviour {

    int width, height;  //The width and height of the tile map

    //A list of colors that represent each tile, the index in the array is the tile index (Will be replaced with textures later).
    //Color[] colors = { Color.white, Color.black, Color.red, Color.blue };

    //A list that determines if any given tile is collidable. The index in the array is the tile index.
    bool[] collidable = { false, false, true, false };

    public static EditorTileController _instance;

    public GameObject tilePrefab; //A referance to the prefab used to represent tiles.
    public TextAsset mapFile; //A reference to the map file to be loaded.
    public GameObject spritePreview;
    Sprite[] sprite;
    int[,] tiles;
    bool[,] collision;
    int[,] rotation;
    int selectedTile = 0;
    string path = "";
    int selectedRotation = 0;
    bool collisionMode = false;

    void Start() {
        if(_instance != null){
            Destroy(gameObject);
            return;
        }
        _instance = this;
        sprite = Resources.LoadAll<Sprite>("Sprites/Tiles/TileSet");
        //LoadMapData();
        //GenerateTileMap(); //Generate the tile map as soon as the game starts.
        SetTile(0);
    }

    
    void Update() {
        if(Input.GetButtonDown("Rotate")){
            selectedRotation = (selectedRotation + 1) % 4;
            spritePreview.transform.Rotate(0, 0, 90);
        }
        if(Input.GetButtonDown("Jump")){
            collisionMode = !collisionMode;
            GenerateTileMap();
        }
        /*if(Input.GetButtonDown("Fire3")){
            string path = EditorUtility.SaveFilePanel("output file", "", "NewMap.txt", "txt");
            string data = "";
            data += width + "," + height;
            for(int y = 0; y < height; y++){
                for(int x = 0; x < width; x++){
                    data += "," + tiles[x,y];
                }
            }
            for(int y = 0; y < height; y++){
                for(int x = 0; x < width; x++){
                    data += "," + collision[x,y];
                }
            }
            for(int y = 0; y < height; y++){
                for(int x = 0; x < width; x++){
                    data += "," + rotation[x,y];
                }
            }
            File.WriteAllText(path, data);
        }*/
    }

    public void mouseOverTile(int x, int y){

    }

    public void mouseDownTile(int x, int y){
        if(collisionMode)
            collision[x,y] = !collision[x,y];
        else {
            tiles[x, y] = selectedTile;
            rotation[x, y] = selectedRotation;
        }
        RecreateTile(x,y);
    }

    public void mouseUpTile(int x, int y){

    }

    public void CreateMap(int width, int height){
        path = "";
        this.width = width;
        this.height = height;
        tiles = new int[width,height];
        collision = new bool[width,height];
        rotation = new int[width,height];
        GenerateTileMap();
    }

    void LoadMapData() {
        //string data = mapFile.text; //We load the text out of the file.
        string tmppath = EditorUtility.OpenFilePanel("Load File", "", "txt");
        if(tmppath == "") return; 
        path = tmppath;
        string data = File.ReadAllText(path);

        string[] tilesData = data.Split(','); //Here we split it into smaller strings by cutting the string every time there is a ','

        width = int.Parse(tilesData[0]); //Here we load the width of the map from the file
        height = int.Parse(tilesData[1]); //Here we load the height of the map from the file

        tiles = new int[width,height];
        collision = new bool[width,height];
        rotation = new int[width,height];

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                int dataIndex = 2 + ((y * width) + x); //This determines the index in the tiles array that contains our tile's data
                int collidableIndex = dataIndex + (width*height);
                int rotationIndex = collidableIndex + (width*height);
                tiles[x, y] = int.Parse(tilesData[dataIndex]);
                collision[x, y] = tilesData[collidableIndex] == "True";
                rotation[x, y] = int.Parse(tilesData[rotationIndex]);
            }
        }
    }

    //A function that generates the tile map from the map file.
    void GenerateTileMap() {
        //First we iterate over all our children and delete them. This is done in case the map has already been previously generated.
        for(int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }

        //Now iterate over every tile and create it.
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                GameObject go = Instantiate(tilePrefab, new Vector3(x, y, 1), Quaternion.identity, transform); //Here we create a new object from the prefab at the correct x,y position, with us as a parrent.
                go.name = "Tile_" + x + "_" + y; //We set the tile's name to something understandable to simplify debugging.

                SpriteRenderer sr = go.GetComponent<SpriteRenderer>(); //We get the sprite renderer off of the new game object.
                //sr.sprite = sprite;
                int tileType = tiles[x, y]; //This is the type of tile this is. It corresponds to the colors and collidable arrays above.
                //sr.color = colors[tileType]; //Here we set the tile's color to the correct color based on the array.
                sr.sprite = sprite[tileType];
                if(collisionMode && collision[x,y])
                    sr.color = Color.red;
                EditorTile et = go.GetComponent<EditorTile>();
                et.x = x;
                et.y = y;
                go.transform.Rotate(0, 0, 90*rotation[x,y]);

                go.GetComponent<BoxCollider2D>().enabled = true;

            }
        }

    }
    
    void RecreateTile(int x, int y){
        Destroy(transform.Find("tile_"+x+"_"+y));
        GameObject go = Instantiate(tilePrefab, new Vector3(x, y, 1), Quaternion.identity, transform); //Here we create a new object from the prefab at the correct x,y position, with us as a parrent.
        go.name = "Tile_" + x + "_" + y; //We set the tile's name to something understandable to simplify debugging.

        SpriteRenderer sr = go.GetComponent<SpriteRenderer>(); //We get the sprite renderer off of the new game object.
        //sr.sprite = sprite;
        int tileType = tiles[x, y]; //This is the type of tile this is. It corresponds to the colors and collidable arrays above.
        //sr.color = colors[tileType]; //Here we set the tile's color to the correct color based on the array.
        sr.sprite = sprite[tileType];
        if(collisionMode && collision[x,y])
            sr.color = Color.red;
        EditorTile et = go.GetComponent<EditorTile>();
        et.x = x;
        et.y = y;
        go.transform.Rotate(0, 0, 90*rotation[x,y]);

        go.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void SetTile(int tile){
        selectedTile = tile;
        spritePreview.GetComponent<Image>().sprite = sprite[tile];
    }

    public void LoadMap(){
        LoadMapData();
        GenerateTileMap();
    }

    public void NewMap(){
        NewMapPopup.x_string = "100";
        NewMapPopup.y_string = "100";
        NewMapPopup.Init();
    }

    public void SaveMap(){
        if(path == ""){
            SaveMapAs();
            return;
        }
        string data = "";
        data += width + "," + height;
        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                data += "," + tiles[x,y];
            }
        }
        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                data += "," + collision[x,y];
            }
        }
        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                data += "," + rotation[x,y];
            }
        }
        File.WriteAllText(path, data);
    }

    public void SaveMapAs(){
        string tmppath;
        if(path == "")
            tmppath = EditorUtility.SaveFilePanel("output file", "", "NewMap.txt", "txt");
        else
            tmppath = EditorUtility.SaveFilePanel("output file", Path.GetDirectoryName(path), Path.GetFileName(path), "txt");
        if(tmppath == "") return;
        path = tmppath;
        SaveMap();
    }

    public void Resize(){
        ResizePopup.x_string = ""+width;
        ResizePopup.y_string = ""+height;
        ResizePopup.Init();
    }

    public void ResizeMap(int width, int height){
        int[,] tiles = new int[width,height];
        bool[,] collision = new bool[width,height];
        int[,] rotation = new int[width,height];
        for(int x = 0; x < Mathf.Min(this.width, width); x++){
            for(int y = 0; y < Mathf.Min(this.height,height); y++){
                tiles[x,y] = this.tiles[x,y];
                collision[x,y] = this.collision[x,y];
                rotation[x,y] = this.rotation[x,y];
            }
        }
        this.tiles = tiles;
        this.collision = collision;
        this.rotation = rotation;
        this.width = width;
        this.height = height;
        GenerateTileMap();
    }

}
