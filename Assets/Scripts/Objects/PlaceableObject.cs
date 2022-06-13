using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlaceableObject : MonoBehaviour {

    public string prefabName;
    public bool enableCollision = true;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public virtual void ShowEditorSprites(){

    }

    public virtual void LoadFromString(string data){
        string[] tokens = data.Split('/');
        prefabName = tokens[0];
        this.transform.position = new Vector3(int.Parse(tokens[1]), int.Parse(tokens[2]), 0);
        enableCollision = tokens[3] == "True";
        GetComponentInChildren<BoxCollider2D>().enabled = enableCollision;
    }

    public virtual string SaveToString(){
        string data = "";
        data += prefabName + "/" + transform.position.x + "/" + transform.position.y + "/" + enableCollision;
        return data;
    }

    public virtual void OpenEditorDialog(){
        PropertiesPanel.Init(enableCollision, this);
    }

    public void ApplyProperties(PropertiesPanel props){
        enableCollision = props.enableCollision;
    }

    public class PropertiesPanel : EditorWindow {

        public bool enableCollision;
        public PlaceableObject placeable;

        public static void Init(bool collision, PlaceableObject placeable){
            PropertiesPanel window = ScriptableObject.CreateInstance<PropertiesPanel>();
            window.enableCollision = collision;
            window.placeable = placeable;
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 170);
            window.ShowPopup();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Object Properties:", EditorStyles.wordWrappedLabel);
            GUILayout.Space(20);
            enableCollision = GUILayout.Toggle(enableCollision, "isColidable");
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
