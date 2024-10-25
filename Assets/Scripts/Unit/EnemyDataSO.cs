using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject {
    public string enemyName;
    public int baseHealth;
    public float baseSpeed;
    public int goldOnDeath;
    public GameObject enemyPrefab;

}
