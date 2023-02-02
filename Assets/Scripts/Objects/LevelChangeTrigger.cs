using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelChangeTrigger : PlaceableObject {

    public string levelName;
    public int x, y;

    void Start() {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        TileController._instance.GenerateTileMap(levelName);
        FindObjectOfType<PlayerController>().transform.position = new Vector3(x, y, 0);
    }

    public override void ShowEditorSprites(){
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public override void LoadFromString(string data, bool isEditor){
        string[] tokens = data.Split('/');
        x = int.Parse(tokens[0]);
        y = int.Parse(tokens[1]);
        levelName = tokens[2];
        data = tokens[3];
        //levelName = tokens[0];
        //data = tokens[1];
        for(int i = 4; i < tokens.Length; i++){
            data += "/" + tokens[i];
        }
        base.LoadFromString(data, isEditor);
    }

    public override string SaveToString(){
        string data  = x + "/" + y + "/" + levelName + "/" + base.SaveToString();
        return data;
    }

    public override void OpenEditorDialog(){
        PropertiesPanelLCT.Init(this);
    }

    public override void ApplyProperties(PropertiesPanel props){
        base.ApplyProperties(props);
        PropertiesPanelLCT lctProps = (PropertiesPanelLCT)props;
        levelName = lctProps.levelName;
        x = lctProps.x;
        y = lctProps.y;
    }

    public class PropertiesPanelLCT : PropertiesPanel {

        public string levelName;
        public int x, y;

        public static void Init(LevelChangeTrigger placeable){
            PropertiesPanelLCT window = ScriptableObject.CreateInstance<PropertiesPanelLCT>();
            window.InitWindow(placeable);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 170);
            window.ShowPopup();
        }

        public override void InitWindow(PlaceableObject placeable){
            base.InitWindow(placeable);
            levelName = ((LevelChangeTrigger)placeable).levelName;
            x = ((LevelChangeTrigger)placeable).x;
            y = ((LevelChangeTrigger)placeable).y;
        }

        public override void OnGUI()
        {
            EditorGUILayout.LabelField("levelName:", EditorStyles.wordWrappedLabel);
            levelName = GUILayout.TextField(levelName, 25);
            EditorGUILayout.LabelField("Player Spawn X:", EditorStyles.wordWrappedLabel);
            x = EditorGUILayout.IntField(x);
            EditorGUILayout.LabelField("Player Spawn Y:", EditorStyles.wordWrappedLabel);
            y = EditorGUILayout.IntField(y);
            base.OnGUI();
        }
    }
}
