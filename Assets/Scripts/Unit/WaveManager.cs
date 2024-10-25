using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    public EnemyData enemyData;
    public int enemiesPerWave = 5;
    public float timeBetweenEnemies = 0.5f;

    private int currentWave = 1;
    private bool isSpawning = false;
    public Player player;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !isSpawning) {
            StartCoroutine(SpawnWave());
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            ResetWave(); // Reset the wave to 1 when 'R' is pressed
        }
    }

    private IEnumerator SpawnWave() {
        isSpawning = true;
        if (player != null) {
            player.SetCurrentWave(currentWave);
        }
        for (int i = 0; i < enemiesPerWave; i++) {
            SpawnEnemy(currentWave);
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
        currentWave++;
        player.EarnIceBlock(2);
        isSpawning = false;
    }

    private void SpawnEnemy(int waveLevel) {
        GameObject enemyPrefab = enemyData.enemyPrefab;
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Unit enemyScript = enemy.GetComponent<Unit>();
        enemyScript.Initialize(enemyData, waveLevel);

        enemyScript.onDeath.AddListener(player.EarnGold);
        enemyScript.onReachedGoal.AddListener(player.LoseLife);
    }

    private void ResetWave() {
        currentWave = 1; // Reset the current wave to 1
        if (player != null) {
            player.SetCurrentWave(currentWave); // Update the player's wave to 1
        }
        Debug.Log("Wave reset to 1");
    }
}
