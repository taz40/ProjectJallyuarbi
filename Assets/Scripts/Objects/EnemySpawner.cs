using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public string entityType;
    private GameObject prefab;

    // Start is called before the first frame update
    void Start(){

        prefab = Resources.Load<GameObject>("Prefabs/Entities/"+entityType);
        if(prefab == null) {
            Debug.LogError("Entity " + entityType + " does not exist.");
            Destroy(gameObject);
        } else {
            summon();
        }
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    void summon() {
        Instantiate(prefab, transform.position, transform.rotation, transform);
    }
}
