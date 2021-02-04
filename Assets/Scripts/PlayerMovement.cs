using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float playerSpeed = 20.0f;

    Rigidbody2D body;
    float horizontal;
    float vertical;
    float moveLimiter = 1f;

    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        
    }

    private void FixedUpdate() {

        Vector2 dir = new Vector2(horizontal, vertical).normalized;

        body.velocity = dir * playerSpeed;
    }
}
