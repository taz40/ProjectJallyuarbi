using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogControler : MonoBehaviour {

    public TextAsset story;
    Story ink;
    public Text text;
    public GameObject button;
    public GameObject buttonList;

    // Start is called before the first frame update
    void Start() {
        ink = new Story(story.text);
        ink.variablesState["players_name"] = "Bob";
        ink.variablesState["npc_name"] = "Smith";
        showLine();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Continue") && ink.currentChoices.Count == 0) {
            showLine();
        }
    }

    private void showLine() {
        if (ink.canContinue) {
            text.text = ink.Continue();
            if (ink.currentTags.Contains("end")) {
                text.text = "end of conversation";
            }else if(ink.currentTags.Contains("buy weapon")) {
                Debug.Log("Buy weapons");
            } else if (ink.currentTags.Contains("buy armor")) {
                Debug.Log("Buy weapons");
            }
        }

        if(ink.currentChoices.Count > 0) {
            showChoices();
        }
    }

    private void showChoices() {
        for(int i = 0; i < ink.currentChoices.Count; i++) {
            GameObject go = Instantiate(button, buttonList.transform);
            go.GetComponentInChildren<Text>().text = ink.currentChoices[i].text;
            int index = i;
            go.GetComponent<Button>().onClick.AddListener(() => { choose(index); });
        }
    }

    private void choose(int index) {
        for(int i = 0; i < buttonList.transform.childCount; i++) {
            Destroy(buttonList.transform.GetChild(i).gameObject);
        }
        ink.ChooseChoiceIndex(index);
        showLine();
    }
}
