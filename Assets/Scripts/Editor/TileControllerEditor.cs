using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileController))]
public class TileControllerEditor : Editor {

    private TileController script;

    private void OnEnable(){
        script = (TileController)target;
    }

    public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Generate Map")) {
			script.GenMapEdit();
		}

		// Draw default inspector after button...
		base.OnInspectorGUI();
	}

}
