using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float playerSpeed = 5;

    void Start() {

    }

    void Update() {
        Vector3 movement = Vector3.zero;
        if(Input.GetAxis("Vertical") > 0) {
            movement.y = 1;
        }else if(Input.GetAxis("Vertical") < 0) {
            movement.y = -1;
        }

        if(Input.GetAxis("Horizontal") > 0) {
            movement.x = 1;
        }else if(Input.GetAxis("Horizontal") < 0) {
            movement.x = -1;
        }

        transform.Translate(movement * playerSpeed * Time.deltaTime);
        
    }
}
