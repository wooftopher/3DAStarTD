using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "ScriptableObjects/TowerData")]

public class TowerDataSO : ScriptableObject {

    public string towerName;
    public float damage;
    public float range;
    public float shootCooldown;
    public int price;
    public Sprite sprite;

}
