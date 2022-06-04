using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using Object = Ink.Runtime.Object;

public class DialogControler : MonoBehaviour {

    Story ink;
    public Text text;
    public Text name;
    public GameObject button;
    public GameObject buttonList;
    public GameObject canvas;
    PlayerController player;
    public GameObject continueText;
    bool textAnim = false;
    string textToAnim = "";
    int character = 0;
    float timer = 0;
    public Dictionary<string, object> globalVars;
    bool running = false;

    void Start() {
        globalVars = new Dictionary<string, object>();
        globalVars["g_players_name"] = "Bob";
        player = FindObjectOfType<PlayerController>();
    }

    public void playStory(Story s) {
        //Load the story, set variables, get speeker name and show the first line of dialog
        ink = s;
        ink.ChoosePathString("start");
        foreach (string key in globalVars.Keys) {
            if(ink.variablesState.GlobalVariableExistsWithName(key))
                ink.variablesState[key] = globalVars[key];
        }
        canvas.SetActive(true);
        showLine();
        player.blockInput();
        text.text = "";
        running = true;
    }

    void Update() {
        if (running) {
            if (textAnim) {
                //Animate Text
                if (textToAnim.Length == 0)
                    textAnim = false;
                timer += Time.deltaTime;
                if (timer >= .01) {
                    timer = 0;
                    character++;
                    text.text = textToAnim.Substring(0, character);
                    if (character >= textToAnim.Length - 1) {
                        textAnim = false;
                        postAnim();
                    }
                }
            } else {
                //If someone presses the continue button and there is not choice to make, show the next line
                if (Input.GetButtonDown("Continue") && ink.currentChoices.Count == 0) {
                    showLine();
                }
            }
        }
    }
    
    private void showLine() {
        //Show the line on the screen if there is not a choice to be made.
        if (ink.canContinue) {
            //text.text = ink.Continue();
            textToAnim = ink.Continue();
            character = 0;
            textAnim = true;
            timer = 0;
            foreach (string key in ink.variablesState) {
                if (key.StartsWith("g_")) {
                    globalVars[key] = ink.variablesState[key];
                }
            }
            name.text = globalVars["g_speeker"].ToString();
            continueText.SetActive(false);
            if (ink.currentTags.Contains("end")) {
                canvas.SetActive(false);
                textAnim = false;
                character = 0;
                timer = 0;
                textToAnim = "";
                running = false;
                player.unblockInput();
            }
        }
    }

    private void postAnim() {
        //If there is a choice to be made, show the choice to the player.
        if (ink.currentChoices.Count > 0) {
            showChoices();
        } else {
            continueText.SetActive(true);
        }

        if (ink.currentTags.Contains("buy weapon")) {
            Debug.Log("Buy weapons");
        } else if (ink.currentTags.Contains("buy armor")) {
            Debug.Log("Buy armor");
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
