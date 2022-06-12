using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Transform TileList;
    public GameObject TileElement;

    // Start is called before the first frame update
    void Start() {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Tiles/TileSet");
        Debug.Log(sprites.Length);
        for(int i = 0; i < sprites.Length; i++){
            GameObject go = Instantiate(TileElement, TileList);

            Image sr = go.GetComponent<Image>();
            sr.sprite = sprites[i];
            int index = i;
            go.GetComponent<Button>().onClick.AddListener(() => { EditorTileController._instance.SetTile(index); });

        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
