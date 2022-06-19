using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int maxHealth;
    public int currentHealth;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(currentHealth <= 0){
            Destroy(gameObject);
        }    
    }

    public void Heal(int amount){
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    public void Damage(int amount){
        currentHealth = Mathf.Max(0, currentHealth - amount);
    }


}
