using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "ScriptableObjects/TowerData")]

public class TowerDataSO : ScriptableObject {

    public string Name;
    public float Damage;
    public float Range;
    public float ShootCoolDown;
    public int price;
    public Sprite Sprite;

}
