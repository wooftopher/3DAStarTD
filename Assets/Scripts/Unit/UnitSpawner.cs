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
        // Calculate the spawn position relative to the adjusted position in the editor
        Vector3 spawnPosition = transform.position;  // Start with the spawner's position
        
        // Optionally adjust the spawn position if needed
        // Example: Offset by 0.5 units in the y-axis
        spawnPosition += new Vector3(0, 0.25f, 0);  // Adjust as needed
        
        GameObject newUnit = Instantiate(UnitPrefab, spawnPosition, Quaternion.identity);
        
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
