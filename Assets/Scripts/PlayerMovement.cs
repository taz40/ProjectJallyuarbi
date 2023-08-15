using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    //The speed the player moves
    public float playerSpeed = 20.0f;

    Rigidbody2D body;
    float horizontal;
    float vertical;
    float moveLimiter = 1f;

    void Start() {
        //get the rigidbody
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {
        //set horizontal and vertical to the axis input.
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        
    }

    private void FixedUpdate() {
        //put horizontal and vertical into a vector and normalize it, then send it to the ridgid body and multiply by the player speed.
        Vector2 dir = new Vector2(horizontal, vertical).normalized;

        body.velocity = dir * playerSpeed;
    }
}
