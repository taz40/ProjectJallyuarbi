using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Transform TileList;
    public GameObject TileElement;

    // Start is called before the first frame update
    void Start() {
        OpenTilesMenu();
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void ClearMenu(){
        foreach(Transform child in TileList){
            Destroy(child.gameObject);
        }
    }

    public void OpenTilesMenu() {
        ClearMenu();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Tiles/TileSet");
        for(int i = 0; i < sprites.Length; i++){
            GameObject go = Instantiate(TileElement, TileList);

            Image sr = go.GetComponent<Image>();
            sr.sprite = sprites[i];
            int index = i;
            go.GetComponent<Button>().onClick.AddListener(() => { EditorTileController._instance.SetTile(index); });

        }
    }

    public void OpenObjectsMenu(){
        ClearMenu();
        GameObject[] gameObjects = Resources.LoadAll<GameObject>("Prefabs/Objects/");
        for(int i = 0; i < gameObjects.Length; i++){
            GameObject go = Instantiate(TileElement, TileList);

            Image sr = go.GetComponent<Image>();
            sr.sprite = gameObjects[i].GetComponentInChildren<SpriteRenderer>().sprite;
            int index = i;
            go.GetComponent<Button>().onClick.AddListener(() => { EditorTileController._instance.SetObject(gameObjects[index]); });

        }
    }

    public void OpenEntitiesMenu() {
        ClearMenu();
        GameObject[] gameObjects = Resources.LoadAll<GameObject>("Prefabs/Entities/");
        for (int i = 0; i < gameObjects.Length; i++) {
            GameObject go = Instantiate(TileElement, TileList);

            Image sr = go.GetComponent<Image>();
            sr.sprite = gameObjects[i].GetComponentInChildren<SpriteRenderer>().sprite;
            int index = i;
            go.GetComponent<Button>().onClick.AddListener(() => { EditorTileController._instance.SetObject(gameObjects[index]); });
        }
    }

    public void OpenNPCMenu() {

    }
}
