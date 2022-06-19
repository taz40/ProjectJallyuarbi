using UnityEngine;
using UnityEditor;

public class NewMapPopup : EditorWindow {

    public static string x_string = "10";
    public static string y_string = "10";

    public static void Init()
    {
        NewMapPopup window = ScriptableObject.CreateInstance<NewMapPopup>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 170);
        window.ShowPopup();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Set the map size:", EditorStyles.wordWrappedLabel);
        GUILayout.Space(20);
        EditorGUILayout.LabelField("X:", EditorStyles.wordWrappedLabel);
        x_string = GUILayout.TextField(x_string, 25);
        EditorGUILayout.LabelField("Y:", EditorStyles.wordWrappedLabel);
        y_string = GUILayout.TextField(y_string, 25);
        if (GUILayout.Button("Create")) {
            EditorTileController._instance.CreateMap(int.Parse(x_string), int.Parse(y_string));
            this.Close();
        }
        if (GUILayout.Button("Cancel")) {
            this.Close();
        }
    }
}