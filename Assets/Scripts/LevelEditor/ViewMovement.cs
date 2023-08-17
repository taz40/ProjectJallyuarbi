using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ViewMovement : MonoBehaviour {
    //The speed the player moves
    public float playerSpeed = 20.0f;

    Rigidbody2D body;
    float horizontal;
    float vertical;
    float moveLimiter = 1f;
    Camera cam;
    private Vector3 dragOrigin;

    void Start() {
        //get the rigidbody
        body = GetComponent<Rigidbody2D>();
        cam = GetComponent<Camera>();
    }

    void Update() {
        //set horizontal and vertical to the axis input.
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        float zoom = Input.GetAxisRaw("Mouse ScrollWheel");
        if (EventSystem.current.IsPointerOverGameObject()) return;
        cam.orthographicSize += zoom * -1 * cam.orthographicSize;

        if (Input.GetButtonDown("Fire2")) {
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetButton("Fire2")) {
            Vector3 pos = cam.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            dragOrigin = Input.mousePosition;
            float speed = -2 * cam.orthographicSize;
            Vector3 move = new Vector3(pos.x * speed * cam.aspect, pos.y * speed, 0);
            transform.Translate(move, Space.World);
        }
    }

    private void FixedUpdate() {
        //put horizontal and vertical into a vector and normalize it, then send it to the ridgid body and multiply by the player speed.
        Vector2 dir = new Vector2(horizontal, vertical).normalized;

        body.velocity = dir * playerSpeed;
    }
}
