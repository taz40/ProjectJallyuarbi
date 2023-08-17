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

    enum EditorMode { 
        MOVE, TILE, OBJECT, COLLISION, PATH
    }

    public int width, height;  //The width and height of the tile map

    //A list of colors that represent each tile, the index in the array is the tile index (Will be replaced with textures later).
    //Color[] colors = { Color.white, Color.black, Color.red, Color.blue };

    //A list that determines if any given tile is collidable. The index in the array is the tile index.
    bool[] collidable = { false, false, true, false };

    public static EditorTileController _instance;

    public GameObject tilePrefab; //A referance to the prefab used to represent tiles.
    public TextAsset mapFile; //A reference to the map file to be loaded.
    public GameObject spritePreview;
    public GameObject selectionPrefab;
    public Slider slider;
    public Text zLevelText;
    public Text PositionText;
    Sprite[] sprite;
    int[,] tiles;
    bool[,] collision;
    int[,] rotation;
    int selectedTile = 0;
    string path = "";
    int selectedRotation = 0;
    //bool collisionMode = false;
    GameObject hoverIndicator;
    int startX, startY;
    int overX, overY;
    List<GameObject> selectionIndicators = new List<GameObject>();
    bool dragging = false;
    bool selectedCollisionState = true;
    //bool objectMode = false;
    GameObject objectToPlace;
    List<GameObject> objects = new List<GameObject>();
    GameObject selectedObject;
    float zLevel;
    int CurrentLayer = 0;
    EditorMode mode = EditorMode.MOVE;

    void Start() {
        if(_instance != null){
            Destroy(gameObject);
            return;
        }
        _instance = this;
        sprite = Resources.LoadAll<Sprite>("Sprites/Tiles/TileSet");
        //LoadMapData();
        //GenerateTileMap(); //Generate the tile map as soon as the game starts.
        //SetTile(0);
        hoverIndicator = Instantiate(selectionPrefab);
        hoverIndicator.SetActive(false);
    }

    
    void Update() {
        if(Input.GetButtonDown("Rotate")){
            if (mode == EditorMode.TILE) {
                selectedRotation = (selectedRotation + 1) % 4;
                spritePreview.transform.Rotate(0, 0, 90);
            }
        }
        if(Input.GetButtonDown("Jump") && mode != EditorMode.PATH){
            if (mode == EditorMode.COLLISION) {
                mode = EditorMode.MOVE;
                spritePreview.GetComponent<Image>().sprite = null;
                if (selectedObject != null) {
                    selectedObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    selectedObject = null;
                }
            } else {
                mode = EditorMode.COLLISION;
                spritePreview.GetComponent<Image>().sprite = null;
                if (selectedObject != null) {
                    selectedObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    selectedObject = null;
                }
            }
            CollisionGraphicUpdate();
        }
        if(Input.GetButtonDown("Delete") && selectedObject != null){
            objects.Remove(selectedObject);
            Destroy(selectedObject);
            selectedObject = null;
        }
        if(Input.GetButtonDown("Interact") && selectedObject != null){
            selectedObject.GetComponent<PlaceableObject>().OpenEditorDialog();
        }
        if(Input.GetButtonDown("Fire3")){
            if (mode == EditorMode.TILE) {
                SetTile(tiles[overX, overY]);
                selectedRotation = rotation[overX, overY];
                spritePreview.transform.rotation = Quaternion.identity;
                spritePreview.transform.Rotate(0, 0, 90 * selectedRotation);
            }
        }
        if(Input.GetButtonDown("Cancel")) {
            mode = EditorMode.MOVE;
            spritePreview.GetComponent<Image>().sprite = null;
            if (selectedObject != null) {
                selectedObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                selectedObject = null;
            }
            CollisionGraphicUpdate();
        }
    }

    public void mouseOverTile(int x, int y){
        if(!dragging) hoverIndicator.SetActive(true);
        hoverIndicator.transform.position = new Vector3(x, y, 0);
        PositionText.text = x + ", " + y;
        if(overX != x || overY != y) {
            overX = x;
            overY = y;
            if(dragging) updateSelectionIndicators();
        }
    }

    public void mouseExitTile(int x, int y){
        hoverIndicator.SetActive(false);
    }

    public void mouseDownTile(int x, int y){
        if(mode == EditorMode.OBJECT){
            foreach(GameObject obj in objects){
                if(obj.transform.position.x == x && obj.transform.position.y == y){
                    if(selectedObject != null){
                        selectedObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                        if(selectedObject == obj){
                            selectedObject = null;
                            return;
                        }
                        selectedObject = null;
                    }
                    selectedObject = obj;
                    obj.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                    return;
                }
            }
            if(selectedObject != null){
                selectedObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                selectedObject = null;
            }
            GameObject go = Instantiate(objectToPlace);
            go.transform.position = new Vector3(x, y, zLevel);
            go.GetComponent<PlaceableObject>().prefabName = objectToPlace.name;
            go.GetComponent<PlaceableObject>().zLevel = zLevel;
            go.GetComponent<PlaceableObject>().ShowEditorSprites();
            //go.GetComponentInChildren<SpriteRenderer>().sortingOrder = CurrentLayer++;
            go.GetComponentInChildren<SpriteRenderer>().sortingOrder = height-y;
            objects.Add(go);
        }
        if (mode == EditorMode.TILE || mode == EditorMode.COLLISION) {
            startX = x;
            startY = y;
            overX = x;
            overY = y;
            hoverIndicator.SetActive(false);
            dragging = true;
            if (mode == EditorMode.COLLISION) selectedCollisionState = !collision[x, y];
            updateSelectionIndicators();
        }
    }

    public void mouseUpTile(int x, int y){
        if(mode == EditorMode.OBJECT || mode == EditorMode.PATH || mode == EditorMode.MOVE) return;
        dragging = false;
        hoverIndicator.SetActive(true);
        x = overX;
        y = overY;
        int xStart = Mathf.Min(startX, x);
        int yStart = Mathf.Min(startY, y);
        int xEnd = Mathf.Max(startX, x);
        int yEnd = Mathf.Max(startY, y);
        for(int x1 = xStart; x1 <= xEnd; x1++){
            for(int y1 = yStart; y1 <= yEnd; y1++){
                modifyTile(x1, y1);
            }
        }
        foreach(GameObject obj in selectionIndicators){
            Destroy(obj);
        }
        selectionIndicators = new List<GameObject>();
    }

    void updateSelectionIndicators(){
        foreach(GameObject obj in selectionIndicators){
            Destroy(obj);
        }
        selectionIndicators = new List<GameObject>();

        int x = overX;
        int y = overY;
        int xStart = Mathf.Min(startX, x);
        int yStart = Mathf.Min(startY, y);
        int xEnd = Mathf.Max(startX, x);
        int yEnd = Mathf.Max(startY, y);
        for(int x1 = xStart; x1 <= xEnd; x1++){
            for(int y1 = yStart; y1 <= yEnd; y1++){
                GameObject go = Instantiate(selectionPrefab);
                go.transform.position = new Vector3(x1, y1, 0);
                selectionIndicators.Add(go);
            }
        }
    }

    void modifyTile(int x, int y){
        if(mode == EditorMode.COLLISION)
            collision[x,y] = selectedCollisionState;
        else if(mode == EditorMode.TILE) {
            tiles[x, y] = selectedTile;
            rotation[x, y] = selectedRotation;
        }
        RecreateTile(x,y);
    }

    public void CreateMap(int width, int height){
        path = "";
        this.width = width;
        this.height = height;
        tiles = new int[width,height];
        CurrentLayer = 0;
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                tiles[x,y] = selectedTile;
            }
        }
        collision = new bool[width,height];
        rotation = new int[width,height];
        foreach(GameObject go in objects){
            Destroy(go);
        }
        objects = new List<GameObject>();
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
        foreach(GameObject go in objects){
            Destroy(go);
        }
        objects = new List<GameObject>();

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

        CurrentLayer = 0;
        for(int i = 2+(height*width)*3; i < tilesData.Length; i++){
            data = tilesData[i];
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/"+data.Split('/')[0]));
            go.GetComponent<PlaceableObject>().prefabName = data.Split('/')[0];
            go.GetComponent<PlaceableObject>().LoadFromString(data.Substring(data.IndexOf('/')+1), true);
            go.GetComponent<PlaceableObject>().ShowEditorSprites();
            //go.GetComponentInChildren<SpriteRenderer>().sortingOrder = CurrentLayer++;
            //go.GetComponentInChildren<SpriteRenderer>().sortingOrder = height-y;
            objects.Add(go);
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
                if(mode == EditorMode.COLLISION && collision[x,y])
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
        Destroy(transform.Find("Tile_"+x+"_"+y).gameObject);
        GameObject go = Instantiate(tilePrefab, new Vector3(x, y, 1), Quaternion.identity, transform); //Here we create a new object from the prefab at the correct x,y position, with us as a parrent.
        go.name = "Tile_" + x + "_" + y; //We set the tile's name to something understandable to simplify debugging.

        SpriteRenderer sr = go.GetComponent<SpriteRenderer>(); //We get the sprite renderer off of the new game object.
        //sr.sprite = sprite;
        int tileType = tiles[x, y]; //This is the type of tile this is. It corresponds to the colors and collidable arrays above.
        //sr.color = colors[tileType]; //Here we set the tile's color to the correct color based on the array.
        sr.sprite = sprite[tileType];
        if(mode == EditorMode.COLLISION && collision[x,y])
            sr.color = Color.red;
        EditorTile et = go.GetComponent<EditorTile>();
        et.x = x;
        et.y = y;
        go.transform.Rotate(0, 0, 90*rotation[x,y]);

        go.GetComponent<BoxCollider2D>().enabled = true;
    }

    void CollisionGraphicUpdate() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (collision[x, y])
                    RecreateTile(x, y);
            }
        }
    }

    public void SetTile(int tile){
        selectedTile = tile;
        mode = EditorMode.TILE;
        CollisionGraphicUpdate();
        if (selectedObject != null){
            selectedObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            selectedObject = null;
        }
        spritePreview.GetComponent<Image>().sprite = sprite[tile];
    }

    public void SetObject(GameObject go) {
        mode = EditorMode.OBJECT;
        CollisionGraphicUpdate();
        objectToPlace = go;
        spritePreview.GetComponent<Image>().sprite = go.GetComponentInChildren<SpriteRenderer>().sprite;
        zLevel = go.GetComponentInChildren<PlaceableObject>().zLevel;
        slider.value = zLevel;
        zLevelText.text = zLevel+"";
        spritePreview.transform.eulerAngles = new Vector3(0, 0, 0);
        selectedRotation = 0;
        if(selectedObject != null){
            selectedObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            selectedObject = null;
        }
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
        foreach(GameObject go in objects){
            data += "," + go.GetComponent<PlaceableObject>().prefabName + "/";
            data += go.GetComponent<PlaceableObject>().SaveToString();
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
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                tiles[x,y] = selectedTile;
            }
        }
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

    public void SetZValue(){
        zLevelText.text = ""+slider.value;
        zLevel = slider.value;
    }

}
