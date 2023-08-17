using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEditor.Animations;

public class NPCInteraction : Interactable {

    public string dialogName;
    string name;
    Story ink;
    DialogControler dialogControler;

    // Start is called before the first frame update
    void Start() {

    }

    public void LoadStory(){
        TextAsset dialog = Resources.Load<TextAsset>("Dialog/"+dialogName);
        if(dialog == null){
            Debug.LogError("Error: Dialog [" + dialogName + "] does not exist.");
            return;
        }
        ink = new Story(dialog.text);
        name = ink.variablesState["npc_name"].ToString();
        Debug.Log(ink.variablesState["animator"]);
        AnimatorController controller = Resources.Load<AnimatorController>("Animations/" + ink.variablesState["animator"]);
        GetComponentInChildren<Animator>().runtimeAnimatorController = controller;
        dialogControler = FindObjectOfType<DialogControler>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public override void Interact(GameObject interactor) {
        dialogControler.playStory(ink);
    }
}
