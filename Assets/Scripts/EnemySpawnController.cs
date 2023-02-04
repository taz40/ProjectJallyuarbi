using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    
    public static EnemySpawnController instance = null;
    Dictionary<string, (int, int)> spawnerData = null;

    void Start() {
        if(instance != null){
            Debug.Log("EnemySpawnController instance already exists!");
            Destroy(this.gameObject);
        }else{
            instance = this;
        }
    }

    public (int, int) loadSpawnerData(string spawner){
        return spawnerData[string];
    }

    public void saveSpawnerData(string spawner, (int, int) data){
        spawnerData[string] = data;
    }
   
}
