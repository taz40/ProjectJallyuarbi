using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelChangeTrigger : PlaceableObject {

    public string levelName;

    void Start() {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        TileController._instance.GenerateTileMap(levelName);
    }

    public override void ShowEditorSprites(){
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public override void LoadFromString(string data){
        base.LoadFromString(data);
        string[] tokens = data.Split('/');
        levelName = tokens[4];
    }

    public override string SaveToString(){
        string data = base.SaveToString();
        data += "/" + levelName;
        return data;
    }

    public override void OpenEditorDialog(){
        PropertiesPanel.Init(enableCollision, levelName, this);
    }

    public void ApplyProperties(PropertiesPanel props){
        enableCollision = props.enableCollision;
        levelName = props.levelName;
    }

    public new class PropertiesPanel : EditorWindow {

        public bool enableCollision;
        public string levelName;
        public LevelChangeTrigger placeable;

        public static void Init(bool collision, string levelName, LevelChangeTrigger placeable){
            PropertiesPanel window = ScriptableObject.CreateInstance<PropertiesPanel>();
            window.enableCollision = collision;
            window.placeable = placeable;
            window.levelName = levelName;
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 170);
            window.ShowPopup();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Object Properties:", EditorStyles.wordWrappedLabel);
            GUILayout.Space(20);
            enableCollision = GUILayout.Toggle(enableCollision, "isColidable");
            EditorGUILayout.LabelField("levelName:", EditorStyles.wordWrappedLabel);
            levelName = GUILayout.TextField(levelName, 25);
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
