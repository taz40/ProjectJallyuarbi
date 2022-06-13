using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NPC : PlaceableObject {

    public string dialogName;

    void Start() {
        
    }

    public override void LoadFromString(string data){
        base.LoadFromString(data);
        string[] tokens = data.Split('/');
        dialogName = tokens[4];
        GetComponent<NPCInteraction>().dialogName = dialogName;
        GetComponent<NPCInteraction>().LoadStory();
    }

    public override string SaveToString(){
        string data = base.SaveToString();
        data += "/" + dialogName;
        return data;
    }

    public override void OpenEditorDialog(){
        PropertiesPanel.Init(enableCollision, dialogName, this);
    }

    public void ApplyProperties(PropertiesPanel props){
        enableCollision = props.enableCollision;
        dialogName = props.dialogName;
    }

    public new class PropertiesPanel : EditorWindow {

        public bool enableCollision;
        public string dialogName;
        public NPC placeable;

        public static void Init(bool collision, string dialogName, NPC placeable){
            PropertiesPanel window = ScriptableObject.CreateInstance<PropertiesPanel>();
            window.enableCollision = collision;
            window.placeable = placeable;
            window.dialogName = dialogName;
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 170);
            window.ShowPopup();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Object Properties:", EditorStyles.wordWrappedLabel);
            GUILayout.Space(20);
            enableCollision = GUILayout.Toggle(enableCollision, "isColidable");
            EditorGUILayout.LabelField("dialogName:", EditorStyles.wordWrappedLabel);
            dialogName = GUILayout.TextField(dialogName, 25);
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
