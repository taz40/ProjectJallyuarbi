using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlaceableObject : MonoBehaviour {

    public string prefabName;
    public bool enableCollision = true;
    public float zLevel = 0.5f;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public virtual void ShowEditorSprites(){

    }

    public virtual void LoadFromString(string data, bool isEditor = false){
        string[] tokens = data.Split('/');
        this.transform.position = new Vector3(int.Parse(tokens[0]), int.Parse(tokens[1]), float.Parse(tokens[2]));
        if(isEditor){
            GetComponentInChildren<SpriteRenderer>().sortingOrder = EditorTileController._instance.height - int.Parse(tokens[1]);
        }else{
            GetComponentInChildren<SpriteRenderer>().sortingOrder = TileController._instance.height - int.Parse(tokens[1]);
        }
        zLevel = transform.position.z;
        enableCollision = tokens[3] == "True";
        GetComponentInChildren<BoxCollider2D>().enabled = enableCollision && !isEditor;
    }

    public virtual string SaveToString(){
        string data = "";
        data += transform.position.x + "/" + transform.position.y + "/" + zLevel + "/" + enableCollision;
        return data;
    }

    public virtual void OpenEditorDialog(){
        PropertiesPanel.Init(this);
    }

    public virtual void ApplyProperties(PropertiesPanel props){
        enableCollision = props.enableCollision;
        zLevel = props.zLevel;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, zLevel);
    }

    public class PropertiesPanel : EditorWindow {

        public bool enableCollision;
        public float zLevel;
        public PlaceableObject placeable;

        public static void Init(PlaceableObject placeable){
            PropertiesPanel window = ScriptableObject.CreateInstance<PropertiesPanel>();
            window.InitWindow(placeable);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 170);
            window.ShowPopup();
        }

        public virtual void InitWindow(PlaceableObject placeable){
            this.placeable = placeable;
            enableCollision = placeable.enableCollision;
            zLevel = placeable.zLevel;
        }

        public virtual void OnGUI() {
            enableCollision = GUILayout.Toggle(enableCollision, "isColidable");
            zLevel = EditorGUILayout.Slider("Z-Level", zLevel, 0, 1);
            if (GUILayout.Button("Apply")) {
                placeable.ApplyProperties(this);
                this.Close();
            }
            if (GUILayout.Button("Cancel")) {
                this.Close();
            }
        }
    }

}
