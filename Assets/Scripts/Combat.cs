using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {

    public float coolDown;
    float timer;
    int damageAmount = 0;
    List<GameObject> alreadyHit;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(timer > 0){
            timer -= Time.deltaTime;
        }
    }

    public void Attack(int amount){
        if(timer > 0)
            return;
        timer = coolDown;
        Collider2D[] targets = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y+0.75f), new Vector2(1, 0.5f), 0);
        foreach(Collider2D target in targets){
            Health health = target.GetComponentInParent<Health>();
            if(health != null && health.gameObject != gameObject)
                health.Damage(amount);
        }
    }
}
