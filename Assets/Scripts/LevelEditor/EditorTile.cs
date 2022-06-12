using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorTile : MonoBehaviour {

    public int x, y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver(){
        if (EventSystem.current.IsPointerOverGameObject()) return;
        EditorTileController._instance.mouseOverTile(x,y);
    }

    void OnMouseUp(){
        if (EventSystem.current.IsPointerOverGameObject()) return;
        EditorTileController._instance.mouseUpTile(x,y);
    }

    void OnMouseDown(){
        if (EventSystem.current.IsPointerOverGameObject()) return;
        EditorTileController._instance.mouseDownTile(x,y);
    }

    void OnMouseExit(){
        if (EventSystem.current.IsPointerOverGameObject()) return;
        EditorTileController._instance.mouseExitTile(x,y);
    }
}
