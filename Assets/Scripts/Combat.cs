using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {

    public Collider2D attackCollider;
    public float attackTime;
    float timer;
    int damageAmount = 0;
    List<GameObject> alreadyHit;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(timer <= 0){
            attackCollider.enabled = false;
        }else{
            timer -= Time.deltaTime;
        }
    }

    public void Attack(int amount){
        alreadyHit = new List<GameObject>();
        attackCollider.enabled = true;
        timer = attackTime;
        damageAmount = amount;
    }

    void OnTriggerStay2D(Collider2D other){
        if(alreadyHit.Contains(other.gameObject))
            return;
        alreadyHit.Add(other.gameObject);
        Health health = other.GetComponentInParent<Health>();
        if(health != null)
            health.Damage(damageAmount);
    }

}
