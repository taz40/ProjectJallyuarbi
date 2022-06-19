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

    public override void LoadFromString(string data, bool isEditor){
        string[] tokens = data.Split('/');
        levelName = tokens[0];
        data = tokens[1];
        for(int i = 2; i < tokens.Length; i++){
            data += "/" + tokens[i];
        }
        base.LoadFromString(data, isEditor);
    }

    public override string SaveToString(){
        string data  = levelName + "/" + base.SaveToString();
        return data;
    }

    public override void OpenEditorDialog(){
        PropertiesPanelLCT.Init(enableCollision, levelName, this);
    }

    public override void ApplyProperties(PropertiesPanel props){
        base.ApplyProperties(props);
        PropertiesPanelLCT lctProps = (PropertiesPanelLCT)props;
        levelName = lctProps.levelName;
    }

    public class PropertiesPanelLCT : PropertiesPanel {

        public string levelName;

        public static void Init(bool collision, string levelName, LevelChangeTrigger placeable){
            PropertiesPanelLCT window = ScriptableObject.CreateInstance<PropertiesPanelLCT>();
            window.InitWindow(placeable);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 170);
            window.ShowPopup();
        }

        public override void InitWindow(PlaceableObject placeable){
            base.InitWindow(placeable);
            levelName = ((LevelChangeTrigger)placeable).levelName;
        }

        public override void OnGUI()
        {
            EditorGUILayout.LabelField("levelName:", EditorStyles.wordWrappedLabel);
            levelName = GUILayout.TextField(levelName, 25);
            base.OnGUI();
        }
    }
}
