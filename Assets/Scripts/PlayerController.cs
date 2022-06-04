using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    PlayerMovement playerMovement;
    Interact interact;

    // Start is called before the first frame update
    void Start() {
        playerMovement = GetComponent<PlayerMovement>();
        interact = GetComponent<Interact>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void blockInput() {
        playerMovement.enabled = false;
        interact.enabled = false;
    }

    public void unblockInput() {
        playerMovement.enabled = true;
        interact.enabled = true;
    }

}
