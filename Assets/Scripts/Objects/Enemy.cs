using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : PlaceableObject {

    List<Vector2> path = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void LoadFromString(string data, bool isEditor = false) {
        path = new List<Vector2>();
        string[] tokens = data.Split('/');
        int points = int.Parse(tokens[0]);
        for(int i = 0; i < points; i++) {
            Vector2 vec = new Vector2();
            vec.x = int.Parse(tokens[i * 2 + 1]);
            vec.y = int.Parse(tokens[i * 2 + 2]);
            path.Add(vec);
        }
        data = tokens[points*2 + 3];
        for (int i = points * 2 + 4; i < tokens.Length; i++) {
            data += "/" + tokens[i];
        }
        base.LoadFromString(data, isEditor);
    }

    public override string SaveToString() {
        string data = "";
        for(int i = 0; i < path.Count; i++) {
            data += path[i].x + "/" + path[i].y + "/";
        }
        data += base.SaveToString();
        return data;
    }

    public override void OpenEditorDialog() {
        PropertiesPanelEnemy.Init(enableCollision, this);
    }

    public override void ApplyProperties(PropertiesPanel props) {
        base.ApplyProperties(props);
        PropertiesPanelEnemy enemyProps = (PropertiesPanelEnemy)props;
    }

    public void EditPath() {
        Debug.Log("Editing Path");
    }

    public class PropertiesPanelEnemy : PropertiesPanel {


        public string dialogName;
        public static void Init(bool collision, Enemy placeable) {
            PropertiesPanelEnemy window = ScriptableObject.CreateInstance<PropertiesPanelEnemy>();
            window.InitWindow(placeable);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 170);
            window.ShowPopup();
        }

        public override void InitWindow(PlaceableObject placeable) {
            base.InitWindow(placeable);
        }

        public override void OnGUI() {
            if(GUILayout.Button("Edit Path")) {
                this.Close();
                (placeable as Enemy).EditPath();
            }
            base.OnGUI();
        }
    }
}
