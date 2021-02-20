using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogControler : MonoBehaviour {

    public TextAsset story;
    Story ink;
    public Text text;
    public Text name;
    public GameObject button;
    public GameObject buttonList;

    void Start() {
        //Load the story, set variables, get speeker name and show the first line of dialog
        ink = new Story(story.text);
        ink.variablesState["players_name"] = "Bob";
        name.text = ink.variablesState["speeker"].ToString();
        showLine();
    }

    void Update() {
        //If someone presses the continue button and there is not choice to make, show the next line
        if (Input.GetButtonDown("Continue") && ink.currentChoices.Count == 0) {
            showLine();
        }
    }
    
    private void showLine() {
        //Show the line on the screen if there is not a choice to be made.
        if (ink.canContinue) {
            text.text = ink.Continue();
            if (ink.currentTags.Contains("end")) {
                text.text = "end of conversation";
            }else if(ink.currentTags.Contains("buy weapon")) {
                Debug.Log("Buy weapons");
            } else if (ink.currentTags.Contains("buy armor")) {
                Debug.Log("Buy armor");
            }
        }

        //If there is a choice to be made, show the choice to the player.
        if(ink.currentChoices.Count > 0) {
            showChoices();
        }
    }

    private void showChoices() {
        //make a new button object for each option the player has, and set its callback to make that choice.
        for(int i = 0; i < ink.currentChoices.Count; i++) {
            GameObject go = Instantiate(button, buttonList.transform);
            go.GetComponentInChildren<Text>().text = ink.currentChoices[i].text;
            int index = i;
            go.GetComponent<Button>().onClick.AddListener(() => { choose(index); });
        }
    }

    private void choose(int index) {
        //Destroy all button objects and tell ink to choose the choice, then show the next line.
        for(int i = 0; i < buttonList.transform.childCount; i++) {
            Destroy(buttonList.transform.GetChild(i).gameObject);
        }
        ink.ChooseChoiceIndex(index);
        showLine();
    }
}
