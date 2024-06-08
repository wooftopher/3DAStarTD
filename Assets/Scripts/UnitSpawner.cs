using UnityEngine;

public class UnitSpawner : MonoBehaviour {
    public GameObject UnitPrefab;
    public float spawnInterval = 2f;
    public int maxEnemies = 10;
    
    bool spawning = false;
    private float spawnTimer;
    private int unitsSpawned = 0;
    public bool showPathGizmos = true;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !spawning) {
            StartSpawning();
        }
    }

    void StartSpawning() {
        spawning = true;
        spawnTimer = Time.time;
        unitsSpawned = 0;
    }

    void SpawnUnit() {
        GameObject newUnit = Instantiate(UnitPrefab, transform.position, Quaternion.identity);
        
        // Set the showPathGizmos variable of the spawned unit
        Unit unitComponent = newUnit.GetComponent<Unit>();
        if (unitComponent != null) {
            unitComponent.showPathGizmos = showPathGizmos;
        }
        
        unitsSpawned++;
        if (unitsSpawned >= maxEnemies) {
            spawning = false;
        }
    }


    void FixedUpdate() {
        if (spawning && Time.time >= spawnTimer + spawnInterval) {
            SpawnUnit();
            spawnTimer = Time.time;
        }
    }
}
