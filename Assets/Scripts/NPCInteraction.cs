using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;


public class NPCInteraction : Interactable {

    public TextAsset inkFile;
    string name;
    Story ink;
    DialogControler dialogControler;

    // Start is called before the first frame update
    void Start() {
        ink = new Story(inkFile.text);
        name = ink.variablesState["npc_name"].ToString();
        dialogControler = FindObjectOfType<DialogControler>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public override void Interact(GameObject interactor) {
        dialogControler.playStory(ink);
    }
}
