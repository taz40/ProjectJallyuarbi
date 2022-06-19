using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NPC : PlaceableObject {

    public string dialogName;

    void Start() {
        
    }

    public override void LoadFromString(string data, bool isEditor = false){
        string[] tokens = data.Split('/');
        dialogName = tokens[0];
        GetComponent<NPCInteraction>().dialogName = dialogName;
        GetComponent<NPCInteraction>().LoadStory();
        data = tokens[1];
        for(int i = 2; i < tokens.Length; i++){
            data += "/" + tokens[i];
        }
        base.LoadFromString(data, isEditor);
    }

    public override string SaveToString(){
        string data  = dialogName + "/" + base.SaveToString();
        return data;
    }

    public override void OpenEditorDialog(){
        PropertiesPanelNPC.Init(enableCollision, dialogName, this);
    }

    public override void ApplyProperties(PropertiesPanel props){
        base.ApplyProperties(props);
        PropertiesPanelNPC npcProps = (PropertiesPanelNPC)props;
        dialogName = npcProps.dialogName;
    }

    public class PropertiesPanelNPC : PropertiesPanel {


        public string dialogName;
        public static void Init(bool collision, string dialogName, NPC placeable){
            PropertiesPanelNPC window = ScriptableObject.CreateInstance<PropertiesPanelNPC>();
            window.InitWindow(placeable);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 170);
            window.ShowPopup();
        }

        public override void InitWindow(PlaceableObject placeable) {
            base.InitWindow(placeable);
            dialogName = ((NPC)placeable).dialogName;
        }

        public override void OnGUI()
        {
            EditorGUILayout.LabelField("dialogName:", EditorStyles.wordWrappedLabel);
            dialogName = GUILayout.TextField(dialogName, 25);
            base.OnGUI();
        }
    }
}
