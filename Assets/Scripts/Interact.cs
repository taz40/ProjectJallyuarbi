using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

    public float range = 1.0f;
    float rangeSq;

    // Start is called before the first frame update
    void Start() {
        rangeSq = range * range;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Interact")) {
            foreach (Interactable go in FindObjectsOfType<Interactable>()) {
                if ((go.transform.position - transform.position).sqrMagnitude <= rangeSq) {
                    go.Interact(gameObject);
                    break;
                }
            }
        }
    }
}
